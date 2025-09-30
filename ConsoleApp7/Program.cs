using System;
using System.Collections.Generic;

class User
{
    public string Login;
    public string Password;
    public DateTime BirthDate;
}

class Question
{
    public string Text;
    public List<string> Options;
    public List<int> CorrectIndices;

    public Question()
    {
        Options = new List<string>();
        CorrectIndices = new List<int>();
    }
}

class Quiz
{
    public string Title;
    public string Category;
    public List<Question> Questions;

    public Quiz()
    {
        Questions = new List<Question>();
    }
}

class Result
{
    public string UserLogin;
    public string QuizTitle;
    public int Score;
    public DateTime Date;

    public Result()
    {
        Date = DateTime.Now;
    }
}

class Program
{
    static List<User> users = new List<User>();
    static List<Quiz> quizzes = new List<Quiz>();
    static List<Result> results = new List<Result>();
    static User currentUser = null;
    static Random rnd = new Random();

    static void Main()
    {
        InitQuizzes();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== ВIКТОРИНА ===");

            if (currentUser == null)
            {
                Console.WriteLine("1. Увiйти");
                Console.WriteLine("2. Зареєструватись");
                Console.WriteLine("0. Вихiд");
                Console.Write("Вибір: ");
                string c = Console.ReadLine();

                if (c == "1") Login();
                else if (c == "2") Register();
                else if (c == "0") break;
            }
            else
            {
                Console.WriteLine("Вiтаю, " + currentUser.Login + "!");
                Console.WriteLine("1. Нова вiкторина");
                Console.WriteLine("2. Мої результати");
                Console.WriteLine("3. Топ-20 по вiкторинi");
                Console.WriteLine("4. Вийти з акаунту");
                Console.WriteLine("0. Вихiд з програми");
                Console.Write("Вибiр: ");
                string c = Console.ReadLine();

                if (c == "1") StartNewQuiz();
                else if (c == "2") ShowMyResults();
                else if (c == "3") ShowTop20();
                else if (c == "4") currentUser = null;
                else if (c == "0") break;
            }
        }

        Console.WriteLine("Дякую за гру!");
    }

    static void Register()
    {
        Console.WriteLine("Логiн: ");
        string login = Console.ReadLine();

        for (int i = 0; i < users.Count; i++)
        {
            if (users[i].Login == login)
            {
                Console.WriteLine("Такий користувач вже iснує!");
                Console.ReadKey();
                return;
            }
        }

        Console.WriteLine("Пароль: ");
        string password = Console.ReadLine();

        Console.WriteLine("Дата народження: ");
        DateTime birth;
        DateTime.TryParse(Console.ReadLine(), out birth);

        User u = new User();
        u.Login = login;
        u.Password = password;
        u.BirthDate = birth;
        users.Add(u);

        Console.WriteLine("Реєстрація успiшна!");
        Console.ReadKey();
    }

    static void Login()
    {
        Console.WriteLine("Логiн: ");
        string login = Console.ReadLine();
        Console.WriteLine("Пароль: ");
        string password = Console.ReadLine();

        for (int i = 0; i < users.Count; i++)
        {
            if (users[i].Login == login && users[i].Password == password)
            {
                currentUser = users[i];
                Console.WriteLine("Вхiд виконано!");
                Console.ReadKey();
                return;
            }
        }

        Console.WriteLine("Невiрний логiн або пароль.");
        Console.ReadKey();
    }

    static void StartNewQuiz()
    {
        Console.WriteLine("=== Виберiть роздiл ===");
        Console.WriteLine("1. Iсторiя");
        Console.WriteLine("2. Географiя");
        Console.WriteLine("3. Бiологiя");
        Console.WriteLine("Ваш вибiр: ");
        string cat = Console.ReadLine();

        List<Question> selected = new List<Question>();

        
        
            if (cat == "1") {category = "Iсторiя"}
            else if (cat == "2"){ category = "Географiя"}
            else if (cat == "3") {category = "Бiологiя"}

            for (int i = 0; i < quizzes.Count; i++)
            {
                if (quizzes[i].Category == category)
                {
                    for (int j = 0; j < quizzes[i].Questions.Count; j++)
                    {
                        if (selected.Count < 20)
                            selected.Add(quizzes[i].Questions[j]);
                    }
                }
            }
        

        if (selected.Count == 0)
        {
            Console.WriteLine("У цiй категорiї ще немає вiкторин!");
            Console.ReadKey();
            return;
        }

        int score = 0;
        for (int i = 0; i < selected.Count; i++)
        {
            Console.Clear();
            Console.WriteLine("Питання " + (i + 1) + ": " + selected[i].Text);
            for (int j = 0; j < selected[i].Options.Count; j++)
            {
                Console.WriteLine((j + 1) + ". " + selected[i].Options[j]);
            }

            Console.WriteLine("Ваша відповiдь: ");
            string[] ans = Console.ReadLine().Split(',');
            List<int> chosen = new List<int>();
            for (int j = 0; j < ans.Length; j++)
            {
                int x;
                if (int.TryParse(ans[j], out x))
                {
                    chosen.Add(x - 1);
                }
            }

            bool correct = true;
            if (chosen.Count != selected[i].CorrectIndices.Count)
                correct = false;
            else
            {
                for (int j = 0; j < selected[i].CorrectIndices.Count; j++)
                {
                    if (!chosen.Contains(selected[i].CorrectIndices[j]))
                        correct = false;
                }
            }

            if (correct) score++;
        }


        Result r = new Result();
        r.UserLogin = currentUser.Login;
        r.QuizTitle = (cat == "4") ? "Змiшана" : "Категорiя " + cat;
        r.Score = score;
        results.Add(r);


        List<Result> same = new List<Result>();
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].QuizTitle == r.QuizTitle)
                same.Add(results[i]);
        }

        for (int i = 0; i < same.Count - 1; i++)
        {
            for (int j = 0; j < same.Count - i - 1; j++)
            {
                if (same[j].Score < same[j + 1].Score)
                {
                    Result tmp = same[j];
                    same[j] = same[j + 1];
                    same[j + 1] = tmp;
                }
            }
        }

        int place = -1;
        for (int i = 0; i < same.Count; i++)
        {
            if (same[i] == r) place = i + 1;
        }

        Console.WriteLine("Ваш результат: " + score + " з " + selected.Count);
        Console.WriteLine("Ваше мiсце у таблицi: " + place);
        Console.ReadKey();
    }

    static void ShowMyResults()
    {
        Console.WriteLine("=== Мої результати ===");
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].UserLogin == currentUser.Login)
            {
                Console.WriteLine(results[i].QuizTitle + " - " + results[i].Score + " балiв (" + results[i].Date + ")");
            }
        }
        Console.ReadKey();
    }

    static void ShowTop20()
    {
        Console.WriteLine("Назва вiкторини: ");
        string title = Console.ReadLine();

        List<Result> list = new List<Result>();
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].QuizTitle == title)
            {
                list.Add(results[i]);
            }
        }

        for (int i = 0; i < list.Count - 1; i++)
        {
            for (int j = 0; j < list.Count - i - 1; j++)
            {
                if (list[j].Score < list[j + 1].Score)
                {
                    Result tmp = list[j];
                    list[j] = list[j + 1];
                    list[j + 1] = tmp;
                }
            }
        }

        Console.WriteLine("=== Топ-20 ===");
        int limit;

     
        if (list.Count < 20)
        {
          limit = list.Count;
        }
        else
{
    limit = 20;
}

        for (int i = 0; i < limit; i++)
        {
            Console.WriteLine((i + 1) + ". " + list[i].UserLogin + " - " + list[i].Score);
        }
        Console.ReadKey();
    }



    static void InitQuizzes()
    {
        Quiz history = new Quiz();
        history.Title = "Iсторiя базова";
        history.Category = "Iсторiя";

        Question h1 = new Question();
        h1.Text = "Хто був першим королем Київської Русі?";
        h1.Options.Add("Олег");
        h1.Options.Add("Володимир Великий");
        h1.Options.Add("Ярослав Мудрий");
        h1.CorrectIndices.Add(0);
        history.Questions.Add(h1);

        Question h2 = new Question();
        h2.Text = "У якому році почалася Друга світова війна?";
        h2.Options.Add("1939");
        h2.Options.Add("1941");
        h2.Options.Add("1945");
        h2.CorrectIndices.Add(0);
        history.Questions.Add(h2);

        Question h3 = new Question();
        h3.Text = "Яка імперія збудувала Колізей?";
        h3.Options.Add("Римська");
        h3.Options.Add("Османська");
        h3.Options.Add("Візантійська");
        h3.CorrectIndices.Add(0);
        history.Questions.Add(h3);

        Question h4 = new Question();
        h4.Text = "Хто написав Конституцію США?";
        h4.Options.Add("Джеймс Медісон");
        h4.Options.Add("Бенджамін Франклін");
        h4.Options.Add("Джордж Вашингтон");
        h4.CorrectIndices.Add(0);
        history.Questions.Add(h4);

        Question h5 = new Question();
        h5.Text = "У якому році відбулася битва під Крутами?";
        h5.Options.Add("1918");
        h5.Options.Add("1920");
        h5.Options.Add("1932");
        h5.CorrectIndices.Add(0);
        history.Questions.Add(h5);

        quizzes.Add(history);

        Quiz geo = new Quiz();
        geo.Title = "Географiя базова";
        geo.Category = "Географiя";

        Question g1 = new Question();
        g1.Text = "Яке море найсолоніше?";
        g1.Options.Add("Мертве море");
        g1.Options.Add("Червоне море");
        g1.Options.Add("Каспійське море");
        g1.CorrectIndices.Add(0);
        geo.Questions.Add(g1);

        Question g2 = new Question();
        g2.Text = "Столиця Бразилії?";
        g2.Options.Add("Бразиліа");
        g2.Options.Add("Ріо-де-Жанейро");
        g2.Options.Add("Сан-Паулу");
        g2.CorrectIndices.Add(0);
        geo.Questions.Add(g2);

        Question g3 = new Question();
        g3.Text = "Який материк найменший за площею?";
        g3.Options.Add("Австралія");
        g3.Options.Add("Європа");
        g3.Options.Add("Антарктида");
        g3.CorrectIndices.Add(0);
        geo.Questions.Add(g3);

        Question g4 = new Question();
        g4.Text = "Яка країна має найбільше населення?";
        g4.Options.Add("Китай");
        g4.Options.Add("Індія");
        g4.Options.Add("США");
        g4.CorrectIndices.Add(0);
        geo.Questions.Add(g4);

        Question g5 = new Question();
        g5.Text = "Яке місто називають «містом вітрів»?";
        g5.Options.Add("Чикаго");
        g5.Options.Add("Лондон");
        g5.Options.Add("Токіо");
        g5.CorrectIndices.Add(0);
        geo.Questions.Add(g5);

        quizzes.Add(geo);

        Quiz bio = new Quiz();
        bio.Title = "Бiологiя базова";
        bio.Category = "Бiологiя";

        Question b1 = new Question();
        b1.Text = "Яка молекула зберігає спадкову інформацію?";
        b1.Options.Add("ДНК");
        b1.Options.Add("РНК");
        b1.Options.Add("Білок");
        b1.CorrectIndices.Add(0);
        bio.Questions.Add(b1);

        Question b2 = new Question();
        b2.Text = "Який орган виділяє інсулін?";
        b2.Options.Add("Підшлункова залоза");
        b2.Options.Add("Печінка");
        b2.Options.Add("Нирки");
        b2.CorrectIndices.Add(0);
        bio.Questions.Add(b2);

        Question b3 = new Question();
        b3.Text = "Скільки кісток у скелеті дорослої людини?";
        b3.Options.Add("206");
        b3.Options.Add("208");
        b3.Options.Add("210");
        b3.CorrectIndices.Add(0);
        bio.Questions.Add(b3);

        Question b4 = new Question();
        b4.Text = "Який орган відповідає за зір?";
        b4.Options.Add("Око");
        b4.Options.Add("Вухо");
        b4.Options.Add("Мозок");
        b4.CorrectIndices.Add(0);
        bio.Questions.Add(b4);

        Question b5 = new Question();
        b5.Text = "Який організм є одноклітинним?";
        b5.Options.Add("Амеба");
        b5.Options.Add("Людина");
        b5.Options.Add("Риба");
        b5.CorrectIndices.Add(0);
        bio.Questions.Add(b5);

        quizzes.Add(bio);
    }


}
