using System;
using System.Collections.Generic;
using System.Linq;

namespace WPF_Quiz_Anwendung.Classes
{
    class Quiz
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
    enum QuestionType
    {
        MultipleRightAnswers,
        SingleRightAnswer
    }
    class Question
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
    class Answer
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
}
