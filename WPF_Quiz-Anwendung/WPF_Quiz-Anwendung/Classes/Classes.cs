using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Windows;

namespace WPF_Quiz_Anwendung.Classes
{
	public class LeaderboardEntry
	{
		public string UserName { get; set; }
		public int Score { get; set; }
		public double ScorePercent { get; set; }
		public DateTime Date { get; set; }

		public LeaderboardEntry(string userName, int score, int totalQuestions)
		{
			if (string.IsNullOrWhiteSpace(userName))
				userName = "Anonym";
			
			UserName = userName;
			Score = score;
			ScorePercent = totalQuestions > 0 ? (double)score / totalQuestions * 100 : 0;
			Date = DateTime.Now;
		}

		// Parameterloser Konstruktor für JSON-Deserialisierung
		public LeaderboardEntry()
		{
		}
	}

	public class Quiz
	{
		public string Title { get; set; }
		public List<Question> Questions { get; set; }
		public List<LeaderboardEntry> Leaderboard { get; set; }

		public Quiz(string title, List<Question> questions)
		{
			if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("title darf nicht leer sein");
			if (questions == null) throw new ArgumentNullException();
			Title = title;
			Questions = questions;
			Leaderboard = new List<LeaderboardEntry>();
		}

		// Parameterloser Konstruktor für JSON-Deserialisierung
		public Quiz()
		{
			Leaderboard = new List<LeaderboardEntry>();
		}

		public void AddLeaderboardEntry(string userName, int score)
		{
			var entry = new LeaderboardEntry(userName, score, Questions.Count);
			Leaderboard.Add(entry);
			Leaderboard = Leaderboard.OrderByDescending(e => e.Score).ThenBy(e => e.Date).ToList();
		}

		public List<LeaderboardEntry> GetTopEntries(int count = 10)
		{
			return Leaderboard.Take(count).ToList();
		}
	}

	public enum QuestionType
	{
		MultipleRightAnswers,
		SingleRightAnswer
	}

	public class Question
	{
		public QuestionType Type { get; set; }
		public string Text { get; set; }
		public List<Answer> Answers { get; set; }
		public List<int> CorrectAnswerIndexes { get; set; } = new List<int>();

		public Question(string text, QuestionType type, List<Answer> answers)
		{
			if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("text darf nicht leer sein");
			if (answers == null || answers.Count == 0) throw new ArgumentException("answers darf nicht null oder leer sein");
			Text = text;
			Type = type;
			Answers = answers;
			GetCorrectAnswerIndexes();
		}

		// Parameterloser Konstruktor für JSON-Deserialisierung
		public Question()
		{
			CorrectAnswerIndexes = new List<int>();
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
		public string Text { get; set; }
		public bool IsCorrect { get; set; }

		public Answer(string text, bool isCorrect)
		{
			if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Text darf nicht leer sein");
			Text = text;
			IsCorrect = isCorrect;
		}

		// Parameterloser Konstruktor für JSON-Deserialisierung
		public Answer()
		{
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

		public static Quiz LoadQuizFromFile(out string filePath)
		{
			Quiz resQuiz = null;
			filePath = "";
			var openFileDialog = new Microsoft.Win32.OpenFileDialog
			{
				Filter = "JSON Dateien (*.json)|*.json|Alle Dateien (*.*)|*.*",
				Title = "Quiz Datei öffnen"
			};
			bool? result = openFileDialog.ShowDialog();

			if (result == true && !string.IsNullOrWhiteSpace(openFileDialog.FileName))
			{
				try
				{
					filePath = openFileDialog.FileName;
					string json = File.ReadAllText(filePath, Encoding.UTF8);
					resQuiz = JsonConvert.DeserializeObject<Quiz>(json);
					
					// Sicherstellen, dass Leaderboard initialisiert ist
					if (resQuiz != null && resQuiz.Leaderboard == null)
					{
						resQuiz.Leaderboard = new List<LeaderboardEntry>();
					}

					// CorrectAnswerIndexes neu berechnen nach dem Laden
					if (resQuiz != null && resQuiz.Questions != null)
					{
						foreach (var question in resQuiz.Questions)
						{
							question.GetCorrectAnswerIndexes();
						}
					}
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
	}
}
