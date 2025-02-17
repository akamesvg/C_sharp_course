using System;
using System.Collections.Generic;

class Program
{
    static List<int> kursIDs = new List<int>();
    static List<string> kursNames = new List<string>();
    static List<int> studentID = new List<int>();
    static List<string> studentName = new List<string>();
    static List<List<int>> kursStud = new List<List<int>>();
    static int kursidcnt = 1;
    static int studidcnt = 1;
    
    static void Main()
    {
        while (true) 
        {
            Console.WriteLine("Меню:");
            Console.WriteLine("1. Добавить курс");
            Console.WriteLine("2. Посмотреть курсы");
            Console.WriteLine("3. Удалить курс");
            Console.WriteLine("4. Записать студента на курс");
            Console.WriteLine("5. Показать студентов на курсе");
            Console.WriteLine("6. Удалить студента из курса");
            Console.WriteLine("7. Выход");
            Console.Write("Выберите пункт: ");
            string vibor = Console.ReadLine();
            switch (vibor)
            {
                case "1": AddKurs(); break;
                case "2": ViewKurs(); break;
                case "3": DelKurs(); break;
                case "4": AddStudKurs(); break;
                case "5": ViewStudKurs(); break;
                case "6": DelStudKurs(); break;
                case "7": return;
                default: Console.WriteLine("Неверное значение. Поробуте снова"); break;
            }
        }
    }

    static void AddKurs()
    {
        Console.Write("Введите название курса: ");
        string kursName = Console.ReadLine();
        kursIDs.Add(kursidcnt);
        kursNames.Add(kursName);
        kursStud.Add(new List<int>());
        Console.WriteLine($"Курс добавлен {kursidcnt}");
        kursidcnt++;
    }

    static void ViewKurs()
    {
        if (kursIDs.Count ==0)
        {
            Console.WriteLine("Нет курсов");
            return;
        }
        Console.WriteLine("Список курсов:");
        for(int i=0; i < kursIDs.Count; i++)
        {
            Console.WriteLine($"ID: {kursIDs[i]}, название {kursNames[i]}");
        }
    }

    static void DelKurs()
    {
        Console.Write("Для удаления курса введите его ID: ");
        if (int.TryParse(Console.ReadLine(), out int kursID))
        {
            int index = kursIDs.IndexOf(kursID);
            if (index != -1)
            {
                kursIDs.RemoveAt(index);
                kursNames.RemoveAt(index);
                kursStud.RemoveAt(index);
                Console.WriteLine("Курс удален");
            }
            else
            {
                Console.WriteLine("Курс не найден");
            }
        }
    }

    static void AddStudKurs()
    {
        Console.Write("Введите фамилию студента: ");
        string studName = Console.ReadLine();
        studentID.Add(studidcnt);
        studentName.Add(studName);
        Console.WriteLine($"Студент добавлен {studidcnt}");
        studidcnt++;
        Console.WriteLine("Введите ID курса: ");
        if (int.TryParse(Console.ReadLine(), out int kursID))
        {
            int ind = kursIDs.IndexOf(kursID);
            if (ind != -1)
            {
                kursStud[ind].Add(studidcnt - 1);
                Console.WriteLine("Студент записан на курс");
            }
            else 
            {
                Console.WriteLine("Курс не найден"); 
            }
        }
    }

    static void ViewStudKurs()
    {
        Console.Write("Введите ID курса:");
        if (int.TryParse(Console.ReadLine(), out int kursID))
        {
            int ind = kursIDs.IndexOf(kursID);
            if (ind != -1)
            {
                if(kursStud[ind].Count ==0)
                {
                    Console.WriteLine("На данном курсе нет студентов");
                    return;
                }
                Console.WriteLine("Список студентов: ");
                foreach (var studID in kursStud[ind])
                {
                    int studind = studentID.IndexOf(studID);
                    Console.WriteLine($"ID: {studID}, Фамилия: {studentName[studind]}");
                }
            }
            else
            {
                Console.WriteLine("Курс не найден");
            }
        }        
    }

    static void DelStudKurs()
    {
        Console.Write("Введите ID курса: ");
        if (int.TryParse(Console.ReadLine(), out int kursID))
        {
            int kursind = kursIDs.IndexOf(kursID);
            if (kursind != -1)
            {
                Console.Write("Введите ID студента: ");
                if (int.TryParse(Console.ReadLine(), out int studID))
                {
                    if (kursStud[kursind].Remove(studID))
                    {
                        Console.WriteLine("Студент удален");
                    }
                    else
                    {
                        Console.WriteLine("Студента нет на курсе");
                    }
                }
            }
            else
            {
                Console.WriteLine("Курс не найден");
            }
        }
    }



}//don't remove