using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Windows;

namespace WPF_Quiz_Anwendung.Classes
{
	public class Config
	{
		public static string UserName { get; set; }
		public static Dictionary<string, bool> AccessibilitySettings { get; set; }
    }
	public class Quiz
	{
		public string Title { get; private set; }
		public List<Question> Questions { get; private set; }
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
	public class QuizFileHandler
	{
		public static void SaveQuizToFile(Quiz quiz, string filePath = "")
		{
			if (quiz == null)
				throw new ArgumentNullException(nameof(quiz));

			if (string.IsNullOrWhiteSpace(filePath))
			{
				var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
				{
					Filter = "JSON Dateien (*.json)|*.json|Alle Dateien (*.*)|*.*",
					Title = "Quiz Datei speichern",
					DefaultExt = ".json"
				};

				bool? result = saveFileDialog.ShowDialog();
				if (result != true)
					throw new OperationCanceledException("Dateispeicherung abgebrochen.");

				filePath = saveFileDialog.FileName;
			}

			var settings = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				NullValueHandling = NullValueHandling.Ignore
			};

			string json = JsonConvert.SerializeObject(quiz, settings);
			File.WriteAllText(filePath, json, Encoding.UTF8);
		}

		public static Quiz LoadQuizFromFile(string inp = "")
		{
			bool? result;
            Quiz resQuiz = null;
			if (inp == "")
			{
				var openFileDialog = new Microsoft.Win32.OpenFileDialog
				{
					Filter = "JSON Dateien (*.json)|*.json|Alle Dateien (*.*)|*.*",
					Title = "Quiz Datei öffnen"
				};
				result = openFileDialog.ShowDialog();
				inp = openFileDialog.FileName;
			}
			else result = true;
			if (result == true && !string.IsNullOrWhiteSpace(inp))
			{
				try
				{
					string filePath = inp;
					string json = File.ReadAllText(filePath, Encoding.UTF8);
					resQuiz = JsonConvert.DeserializeObject<Quiz>(json);
				}
				catch (Exception e)
				{
					MessageBox.Show("Fehler beim öffnen der Datei: " + e.Message);
				}
				return resQuiz;
			}
			else
			{
				return null;
			}
		}
		public static void LoadConfigFromFile()
		{
			
        }
		public static void SaveConfigToFile()
		{
			
        }
    }
}
