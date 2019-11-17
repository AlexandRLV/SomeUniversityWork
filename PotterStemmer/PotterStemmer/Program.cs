using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotterStemmer
{
    class Program
    {
        static char[] vowels = new char[] { 'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я' };

        static void Main(string[] args)
        {
            // Получаем входное слово, удаляем лишние слова и пробелы, заменяем все буквы ё на е
            Console.WriteLine("Введите слово");
            string s = Console.ReadLine();
            if (s.Contains(' '))
                s = s.Remove(s.IndexOf(' '), s.Length - s.IndexOf(' '));
            if (s.Contains('ё'))
            {
                s.Replace('ё', 'е');
            }

            // Получаем RV и сохраняем начальные буквы
            string rv = TakeRV(s);
            s = s.Remove(s.IndexOf(rv));

            // Основные шаги
            Console.Write("Шаг 1:");
            // Пробуем удалить Perfective Gerund
            string step = DeletePerfectiveGerund(rv);
            // Если получилось, сохраняем результат
            if (step != rv)
            {
                rv = step;
            }
            else
            {
                // Если не получилось, пробуем удалить Reflexive.
                step = DeleteReflexive(rv);
                if (step != rv)
                {
                    rv = step;
                }
                else
                {
                    // Пробуем удалить Adjectival
                    step = DeleteAdjectival(rv);
                    if (step != rv)
                    {
                        rv = step;
                    }
                    else
                    {
                        // Пробуем удалить Verb
                        step = DeleteVerb(rv);
                        if (step != rv)
                        {
                            rv = step;
                        }
                        else
                        {
                            // Пробуем удалить Noun
                            rv = DeleteNoun(rv);
                        }
                    }
                }
            }
            Console.WriteLine(rv);

            Console.Write("Шаг 2:");
            
            // Если оканчивается на "и" - удаляем "и"
            if (rv.EndsWith("и"))
            {
                rv.Remove(rv.Length - 1);
            }
            Console.WriteLine(rv);

            Console.Write("Шаг 3:");

            // Получаем R2 и сохраняем удалённые буквы
            step = TakeR2(rv);
            string removed = rv.Remove(rv.IndexOf(step));

            // Пробуем удалить Derivational из R2
            string r2 = DeleteDerivational(step);

            // Если получилось, сохраняем результат
            if (r2 != step)
            {
                rv = removed + r2;
            }
            Console.WriteLine(rv);

            Console.Write("Шаг 4:");

            // Пробуем удалить Superlative
            step = DeleteSuperlative(rv);

            // Если заканчивается на "нн" - удаляем последнюю букву.
            if (step.EndsWith("нн"))
            {
                rv = step.Remove(step.Length - 1);
            }
            else
            {
                // Если не заканчивается на "нн", Superlative не удалено, проверяем "ь" и удаляем его, если есть
                if (step == rv && step.EndsWith("ь"))
                {
                    rv = step.Remove(step.Length - 1);
                }
            }
            Console.WriteLine(rv);

            // Возвращаем удалённые буквы и выводим результат
            Console.Write("Результат:");
            Console.WriteLine(s + rv);
        }

        // Получаем RV
        private static string TakeRV(string s)
        {
            // Получаем подстроку, начинающуюся после первой гласной
            return s.Substring(s.IndexOfAny(vowels));
        }

        // ПОлучаем R1
        private static string TakeR1(string s)
        {
            // Получаем RV и проверяем следующую букву, пока не найдём согласную
            string s1 = s.Substring(s.IndexOfAny(vowels));
            bool vowel = false;
            while (!vowel && s1.Length > 0)
            {
                foreach (char l in vowels)
                {
                    if (s1[0] == l)
                    {
                        vowel = true;
                    }
                }
                s1 = s1.Substring(1);
                vowel = !vowel;
            }

            return s1;
        }

        // Получаем R2
        private static string TakeR2(string s)
        {
            return TakeR1(TakeR1(s));
        }

        // Пробуем удалить Perfective Gerund, если ничего не найдено - возвращаем исходную строку
        private static string DeletePerfectiveGerund(string s)
        {
            if (s.EndsWith("ившись") || s.EndsWith("ывшись"))
            {
                return s.Remove(s.Length - 6);
            }
            if (s.EndsWith("вшись") && (s[s.Length - 6] == 'а' || s[s.Length - 6] == 'я'))
            {
                return s.Remove(s.Length - 5);
            }
            if (s.EndsWith("ивши") || s.EndsWith("ывши"))
            {
                return s.Remove(s.Length - 4);
            }
            if (s.EndsWith("вши") && (s[s.Length - 4] == 'а' || s[s.Length - 4] == 'я'))
            {
                return s.Remove(s.Length - 3);
            }
            if (s.EndsWith("ив") || s.EndsWith("ыв"))
            {
                return s.Remove(s.Length - 2);
            }
            if (s.EndsWith("в") && (s[s.Length - 2] == 'а' || s[s.Length - 2] == 'я'))
            {
                return s.Remove(s.Length - 1);
            }
            return s;
        }

        // Пробуем удалить Adjective, если ничего не найдено - возвращаем исходную строку
        private static string DeleteAdjective(string s)
        {
            if (s.EndsWith("ими") || s.EndsWith("ыми") || s.EndsWith("его") || s.EndsWith("ого") || s.EndsWith("ему") || s.EndsWith("ому"))
            {
                return s.Remove(s.Length - 3);
            }
            if (s.EndsWith("ее") || s.EndsWith("ие") || s.EndsWith("ые") || s.EndsWith("ое") || s.EndsWith("ей") || s.EndsWith("ий") || s.EndsWith("ый") || s.EndsWith("ой") || s.EndsWith("ем") || s.EndsWith("им") || s.EndsWith("ым") || s.EndsWith("ом") || s.EndsWith("их") || s.EndsWith("ых") || s.EndsWith("ую") || s.EndsWith("юю") || s.EndsWith("ая") || s.EndsWith("яя") || s.EndsWith("ою") || s.EndsWith("ею"))
            {
                return s.Remove(s.Length - 2);
            }
            return s;
        }

        // Пробуем удалить Participle, если ничего не найдено - возвращаем исходную строку
        private static string DeleteParticiple(string s)
        {
            if (s.EndsWith("ивш") || s.EndsWith("ывш") || s.EndsWith("ующ"))
            {
                return s.Remove(s.Length - 3);
            }
            if ((s.EndsWith("ем") || s.EndsWith("нн") || s.EndsWith("вш") || s.EndsWith("ющ")) && (s[s.Length - 3] == 'а' || s[s.Length - 3] == 'я'))
            {
                return s.Remove(s.Length - 2);
            }
            if (s.EndsWith("щ") && (s[s.Length - 2] == 'а' || s[s.Length - 2] == 'я'))
            {
                return s.Remove(s.Length - 1);
            }
            return s;
        }

        // Пробуем удалить Reflexive, если ничего не найдено - возвращаем исходную строку
        private static string DeleteReflexive(string s)
        {
            if (s.EndsWith("ся") || s.EndsWith("сь"))
            {
                return s.Remove(s.Length - 2);
            }
            return s;
        }

        // Пробуем удалить Verb, если ничего не найдено - возвращаем исходную строку
        private static string DeleteVerb(string s)
        {
            if (s.EndsWith("ейте") || s.EndsWith("уйте"))
            {
                return s.Remove(s.Length - 4);
            }
            if (s.EndsWith("ила") || s.EndsWith("ыла") || s.EndsWith("ена") || s.EndsWith("ите") || s.EndsWith("или") || s.EndsWith("ыли") || s.EndsWith("ило") || s.EndsWith("ыло") || s.EndsWith("ено") || s.EndsWith("ует") || s.EndsWith("уют") || s.EndsWith("ены") || s.EndsWith("ить") || s.EndsWith("ыть") || s.EndsWith("ишь"))
            {
                return s.Remove(s.Length - 3);
            }
            if ((s.EndsWith("ете") || s.EndsWith("йте") || s.EndsWith("ешь") || s.EndsWith("нно")) && (s[s.Length - 4] == 'а' || s[s.Length - 4] == 'я'))
            {
                return s.Remove(s.Length - 3);
            }
            if (s.EndsWith("ей") || s.EndsWith("уй") || s.EndsWith("ил") || s.EndsWith("ыл") || s.EndsWith("ым") || s.EndsWith("им") || s.EndsWith("ен") || s.EndsWith("ят") || s.EndsWith("ит") || s.EndsWith("ыт") || s.EndsWith("ую"))
            {
                return s.Remove(s.Length - 2);
            }
            if ((s.EndsWith("ла") || s.EndsWith("на") || s.EndsWith("ли") || s.EndsWith("ем") || s.EndsWith("ло") || s.EndsWith("но") || s.EndsWith("ет") || s.EndsWith("ют") || s.EndsWith("ны") || s.EndsWith("ть")) && (s[s.Length - 3] == 'а' || s[s.Length - 3] == 'я'))
            {
                return s.Remove(s.Length - 2);
            }
            if (s.EndsWith("ю"))
            {
                return s.Remove(s.Length - 1);
            }
            if ((s.EndsWith("й") || s.EndsWith("л") || s.EndsWith("н")) && (s[s.Length - 2] == 'а' || s[s.Length - 2] == 'я'))
            {
                return s.Remove(s.Length - 1);
            }
            return s;
        }

        // Пробуем удалить Noun, если ничего не найдено - возвращаем исходную строку
        private static string DeleteNoun(string s)
        {
            if (s.EndsWith("иями"))
            {
                return s.Remove(s.Length - 4);
            }
            if (s.EndsWith("ями") || s.EndsWith("ами") || s.EndsWith("ией") || s.EndsWith("иям") || s.EndsWith("ием") || s.EndsWith("иях"))
            {
                return s.Remove(s.Length - 3);
            }
            if (s.EndsWith("ев") || s.EndsWith("ов") || s.EndsWith("ие") || s.EndsWith("ье") || s.EndsWith("еи") || s.EndsWith("ии") || s.EndsWith("ей") || s.EndsWith("ой") || s.EndsWith("ий") || s.EndsWith("ям") || s.EndsWith("ем") || s.EndsWith("ам") || s.EndsWith("ом") || s.EndsWith("ах") || s.EndsWith("ях") || s.EndsWith("ию") || s.EndsWith("ью") || s.EndsWith("ия") || s.EndsWith("ья"))
            {
                return s.Remove(s.Length - 2);
            }
            if (s.EndsWith("а") || s.EndsWith("е") || s.EndsWith("и") || s.EndsWith("й") || s.EndsWith("о") || s.EndsWith("у") || s.EndsWith("ы") || s.EndsWith("ь") || s.EndsWith("ю") || s.EndsWith("я"))
            {
                return s.Remove(s.Length - 1);
            }
            return s;
        }

        // Пробуем удалить Superlative, если ничего не найдено - возвращаем исходную строку
        private static string DeleteSuperlative(string s)
        {
            if (s.EndsWith("ейше"))
            {
                return s.Remove(s.Length - 4);
            }
            if (s.EndsWith("ейш"))
            {
                return s.Remove(s.Length - 3);
            }
            return s;
        }

        // Пробуем удалить Derivational, если ничего не найдено - возвращаем исходную строку
        private static string DeleteDerivational(string s)
        {
            if (s.EndsWith("ость"))
            {
                return s.Remove(s.Length - 4);
            }
            if (s.EndsWith("ост"))
            {
                return s.Remove(s.Length - 3);
            }
            return s;
        }

        // Пробуем удалить Adjectival, если ничего не найдено - возвращаем исходную строку
        private static string DeleteAdjectival(string s)
        {
            string s1 = DeleteAdjective(s);
            if (s1 != s)
                return DeleteParticiple(s1);
            else
                return s;
        }
    }
}
