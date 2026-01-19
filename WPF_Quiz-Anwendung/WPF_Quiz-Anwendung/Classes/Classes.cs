using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Windows;
using System.Globalization;

namespace WPF_Quiz_Anwendung.Classes
{
	public enum Theme {Light, Dark}
	public class Config
	{
        public static string UserName { get; set; } = null;
		public static Theme Theme { get; set; }
		public static string DefaultQuestionPath { get; set; }
    }
	public class Quiz
	{
        public static string currentPath { get; set; }
		public string Title { get; private set; }
		public List<Question> Questions { get; private set; }
        public List<LeaderboardEntry> Leaderboard { get; set; }
        public Quiz(string title, List<Question> questions)
		{
			if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("title darf nicht leer sein");
			if (questions == null) throw new ArgumentNullException();
			Title = title;
			Questions = questions;
		}
	}
	public enum QuestionType
	{
		MultipleRightAnswers,
		SingleRightAnswer
	}
	public class Question
	{
		public QuestionType Type { get; private set; }
		public string Text { get; private set; }
		public List<Answer> Answers { get; private set; }
		public List<int> CorrectAnswerIndexes { get; private set; } = new List<int>();
		public Question(string text, QuestionType type, List<Answer> answers)
		{
			if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("text darf nicht leer sein");
			if (answers == null || answers.Count == 0) throw new ArgumentException("answers darf nicht null oder leer sein");
			Text = text;
			Type = type;
			Answers = answers;
			GetCorrectAnswerIndexes();
		}
		public void GetCorrectAnswerIndexes()
		{
			CorrectAnswerIndexes.Clear();
			for (int index = 0; index < Answers.Count; index++)
			{
				if (Answers[index].IsCorrect)
				{
					CorrectAnswerIndexes.Add(index);
				}
			}

			if (Type == QuestionType.SingleRightAnswer && CorrectAnswerIndexes.Count != 1)
			{
				throw new ArgumentException("SingleRightAnswer-Frage muss genau eine richtige Antwort haben! (Frage: " + Text + ")");
			}
		}
	}
	public class Answer
	{
		public string Text { get; private set; }
		public bool IsCorrect { get; private set; }
		public Answer(string text, bool isCorrect)
		{
			if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Text darf nicht leer sein");
			Text = text;
			IsCorrect = isCorrect;
		}
	}
    public class LeaderboardEntry
    {
        public string Username { get; private set; }
        public double Score { get; private set; }
		public DateTime Timestamp { get; private set; }
		public LeaderboardEntry(double score )
		{
			Score = score;
			Username = Config.UserName;
			Timestamp = DateTime.Now;
		}
        [JsonConstructor]
        private LeaderboardEntry(string username, double score, DateTime timestamp)
        {
            Username = username;
            Score = score;
            Timestamp = timestamp;
        }
    }
    public class QuizFileHandler
	{
        //QUIZFILES
        public static void SaveQuizToFile(Quiz quiz, double score, string filePath = "")
        {
            if (quiz == null)
                throw new ArgumentNullException(nameof(quiz));

            if (quiz.Leaderboard == null)
                quiz.Leaderboard = new List<LeaderboardEntry>();

            quiz.Leaderboard.Add(new LeaderboardEntry(score));

            foreach (var question in quiz.Questions)
            {
                question.GetCorrectAnswerIndexes();
            }

            if (string.IsNullOrWhiteSpace(filePath))
            {
                var dlg = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "JSON Dateien (*.json)|*.json|Alle Dateien (*.*)|*.*",
                    DefaultExt = ".json",
                    Title = "Quiz speichern"
                };

                if (dlg.ShowDialog() != true)
                    throw new OperationCanceledException();

                filePath = dlg.FileName;
            }

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            File.WriteAllText(
                filePath,
                JsonConvert.SerializeObject(quiz, settings),
                Encoding.UTF8);
        }



        public static Quiz LoadQuizFromFile(string path = "")
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                var dlg = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "JSON Dateien (*.json)|*.json|Alle Dateien (*.*)|*.*",
                    Title = "Quiz öffnen"
                };

                if (dlg.ShowDialog() != true)
                    return null;

                path = dlg.FileName;
            }

            Quiz.currentPath = path;

            try
            {
                string json = File.ReadAllText(path, Encoding.UTF8);
                Quiz quiz = JsonConvert.DeserializeObject<Quiz>(json);

                if (quiz.Leaderboard == null)
                    quiz.Leaderboard = new List<LeaderboardEntry>();

                return quiz;
            }
            catch (Exception e)
            {
                MessageBox.Show("Fehler beim Laden: " + e.Message);
                return null;
            }
        }


        //CONFIGFILE
        public static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        public static void LoadConfigFromFile()
        {
            try
            {
                if (!File.Exists(ConfigFilePath))
                    return;

                string json = File.ReadAllText(ConfigFilePath, Encoding.UTF8);
                dynamic data = JsonConvert.DeserializeObject(json);

                Config.UserName = data.UserName;
                Config.DefaultQuestionPath = data.DefaultQuestionPath;

                if (Enum.TryParse(data.Theme?.ToString(), true, out Theme theme))
                {
                    Config.Theme = theme;
                }
                else
                {
                    Config.Theme = Theme.Light;
                }
            }
            catch
            {
                Config.Theme = Theme.Light;
            }
        }


        public static void SaveConfigToFile()
        {
            var configObject = new
            {
                UserName = Config.UserName,
                Theme = Config.Theme,
                DefaultQuestionPath = Config.DefaultQuestionPath
            };

            string json = JsonConvert.SerializeObject(
                configObject,
                Formatting.Indented
            );

            File.WriteAllText(ConfigFilePath, json, Encoding.UTF8);
        }

    }
}
