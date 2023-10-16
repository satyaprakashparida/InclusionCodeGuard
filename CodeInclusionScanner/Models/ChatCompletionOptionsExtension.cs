using Azure.AI.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInclusionScanner.Models
{
    public static class ChatCompletionOptionsExtension
    {
        public static void AddMessage(this ChatCompletionsOptions options, ChatRole role, string message)
        {
            options.Messages.Add(new ChatMessage(role, message));
        }

        public static void ResetOptions(this ChatCompletionsOptions options)
        {
            options.Messages.Clear();
        }
    }
}
