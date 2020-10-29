using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Question.Domain.CreateQuestionWorkflow
{
    public struct CreateQuestionCmd
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string[] Tags { get; set; }
        public string AnswerToTheQuest { get; set; }
       

        public CreateQuestionCmd(string title, string text, string[] tags, string AnswerToTheQuest)
        {
            Title = title;
            Text = text;
            Tags = tags;
            this.AnswerToTheQuest = AnswerToTheQuest;
        }
    }
}