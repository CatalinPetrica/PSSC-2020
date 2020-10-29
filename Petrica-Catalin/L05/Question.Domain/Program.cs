using System;
using System.Collections.Generic;
using Question.Domain.CreateQuestionWorkflow;
using static Question.Domain.CreateQuestionWorkflow.CreateQuestionResult;
using static Question.Domain.CreateQuestionWorkflow.CreateQuestionResult.QuestionCreated;

namespace Test.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmd = new CreateQuestionCmd("How to change the placeholder for the input", "How can i change the css properties for the placeholder in input field", new string[] { "Java", "AngularJS" }, "");
            var result = CreateQuestion(cmd);

            result.Match(
                ProcessQuestionCreated,
                ProcessQuestionNotCreated,
                ProcessInvalidQuestion
            );
            Console.ReadLine();

            if(result.form == true)
            {
                Console.WriteLine("Would you like to give a vote?");
                Console.WriteLine("YES or NO?");
                string decision = Console.ReadLine();
                if (decision.Equals("Y"))
                {
                    Console.WriteLine("Would you like to give a positive or negative vote ? ");
                    string vote = Console.ReadLine();
                    if (vote.Equals("P"))
                        result.getVotes(1);
                    else if (vote.Equals("N"));
                    result.getVotes(-1);
                }


            }
        }
        private static ICreateQuestionResult ProcessQuestionNotCreated(QuestionNotCreated questionNotCreatedResult)
        {
            Console.WriteLine($"Question not created: {questionNotCreatedResult.Feedback}");
            return questionNotCreatedResult;
        }

        private static ICreateQuestionResult ProcessQuestionCreated(QuestionCreated question)
        {
            Console.WriteLine($"Question {question.QuestionId}");
            return question;
        }

        private static ICreateQuestionResult ProcessInvalidQuestion(QuestionValidationFailed validationErrors)
        {
            Console.WriteLine("Question validation failed: ");
            foreach (var error in validationErrors.ValidationErrors)
            {
                Console.WriteLine(error);
            }
            return validationErrors;
        }

        public static ICreateQuestionResult CreateQuestion(CreateQuestionCmd createQuestionCommand)
        {
            if (string.IsNullOrWhiteSpace(createQuestionCommand.Title))
            {
                var errors = new List<string>() { "Title is missing" };
                return new QuestionValidationFailed(errors);
            }

            if (createQuestionCommand.Title.Length < 0 && !string.IsNullOrWhiteSpace(createQuestionCommand.Title))
            {
                var errors = new List<string>() { "Title cannot be shorter than 8 characters." };
                return new QuestionValidationFailed(errors);
            }

            if (createQuestionCommand.Title.Length > 1000)
            {
                var errors = new List<string>() { "Title cannot be longer than 180 characters." };
                return new QuestionValidationFailed(errors);
            }

            if (string.IsNullOrWhiteSpace(createQuestionCommand.Text))
            {
                var errors = new List<string>() { "Body is missing" };
                return new QuestionValidationFailed(errors);
            }

            if (createQuestionCommand.Text.Length < 0 && !string.IsNullOrWhiteSpace(createQuestionCommand.Title))
            {
                var errors = new List<string>() { "Body cannot be shorter than 10 characters." };
                return new QuestionValidationFailed(errors);
            }

            if (createQuestionCommand.Text.Length > 1000)
            {
                var errors = new List<string>() { "Body is limited to 10000 characters; you entered 10005." };
                return new QuestionValidationFailed(errors);
            }

            if (createQuestionCommand.Tags.Length < 1)
            {
                var errors = new List<string>() { "Please enter at least one tag; see a list of popular tags." };
                return new QuestionValidationFailed(errors);
            }

            if (createQuestionCommand.Tags.Length > 3)
            {
                var errors = new List<string>() { "You entered to much tags, you need to delete some of these" };
                return new QuestionValidationFailed(errors);
            }

            if (new Random().Next(10) > 1)
            {
                return new QuestionNotCreated("Question could not be verified");
            }

            var questionId = Guid.NewGuid();
            var result = new QuestionCreated(questionId, createQuestionCommand.Title, "", true);

            if (result.form)
            {
                Console.WriteLine(result.ToString());
            }
            else
            {
                QuestionNotCreated feedback = new QuestionNotCreated("Question was closed, can be created !");
            }

            return result;
        }
    }
}