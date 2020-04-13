using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

/*
 * Algoritmo para Chatbot en Revit
 * 
 * El chatbot deberia seguir los siguientes pasos:
 * 
 * 1- traducir voz a texto
 * 2- dividir texto en acciones
 * 3- parametrizar cada accion
 * 4- ejecutar cada accion
 * 
 * Se trata en este ejercicio de crear un algoritmo capaz de dividir una frase completa en diversas acciones,
 * en funcion de diferentes grupos de palabras clave elegidos previamente
 * 
 * Para otras piezas de la arquitectura que permitiria realizar un chatbot para Revit tenemos entre otras:
 * 
 * https://azure.microsoft.com/en-us/services/cognitive-services/speech-to-text/#overview (traduccion voz a texto)
 * https://azure.microsoft.com/en-us/services/bot-service/ (parametrizacion de texto)
 * 
 * https://aws.amazon.com/es/transcribe/ (traduccion voz a texto)
 * https://aws.amazon.com/es/lex/ (parametrizacion de texto)
 * 
 * https://cloud.google.com/speech-to-text/ (traduccion voz a texto)
 * https://cloud.google.com/dialogflow/ (parametrizacion de texto)
 * 
 * Se persigue que pudiera funcionar con comandos como por ejemplo:
 * 
 * "Vista planta principal estructura"
 * "Crea muro horizontal de tres metros de longitud"
 * "Inserta familia de inodoro de 40 por 60"
 * "Nuevo plano de instalaciones de fontaneria de la planta 5"
 * 
 * El objectivo de la practica es aprender a crear algoritmos, orientacion a objetos y Linq
 */

namespace Algoritmos.Speech
{
    class SpeechMain
    {
        public static void Execute()
        {
            var text = "asdf ghjk clavedos asdf asdf clave tres asdf asdf claveuno asdf clave cuatro asdf asdf";
            //var text = "asdf ghjk clavedos asdf asdf clave tres asdf asdf claveuno asdf clave cuatro";
            //var text = "asdf ghjk clavedos asdf asdf clave tres asdf asdf claveuno asdf claveuno";
            //var text = "claveuno asdf asdf";
            //var text = "claveuno";
            //var text = "clave tres asdf asdf";
            //var text = "clave cuatro";
            //var text = "asdf asff";

            var keys = new string[]
            {
                "claveuno",
                "clavedos",
                "clave tres",
                "clave cuatro"
            };

            Console.WriteLine("--- Flow Sentences");

            var flowSentences = ProcessSentence(text, keys);
            flowSentences.ToList().ForEach(s => Console.WriteLine(s));

            Console.WriteLine();
            Console.WriteLine("--- Oop Sentences");

            var oopSentences = TextElement.ProcessSentence(text, keys);
            oopSentences.ToList().ForEach(e => Console.WriteLine(e.Sentence));

            Console.WriteLine();

            //Console.ReadLine();
        }

        public static IEnumerable<string> ProcessSentence(string text, string[] keys)
        {
            var textSplit = text.Split(' ');
            var maxWords = keys.Max(k => k.Split(' ').Length);

            var positions = new List<int>();

            for (int i = 0; i < textSplit.Length; i++)
            {
                for (int j = 1; j <= maxWords; j++)
                {
                    var words = textSplit.Skip(i).Take(j);
                    var comparator = string.Join(" ", words);
                    if (keys.Contains(comparator) && !positions.Contains(i))
                    {
                        positions.Add(i);
                    }
                }
            }
            positions.Add(textSplit.Length);

            var sentences = new List<string>();

            for (int i = 0; i < positions.Count - 1; i++)
            {
                var startIndex = positions[i];
                var endIndex = positions[i + 1] - positions[i];

                var words = textSplit.Skip(startIndex).Take(endIndex);
                var sentence = string.Join(" ", words);
                sentences.Add(sentence);
            }

            return sentences;
        }
    }
}
