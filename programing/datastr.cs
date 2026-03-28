using System;
using System.Collections.Generic;

namespace ConstructionSiteApp
{
    public interface IIdentifiable
    {
        int Id { get; }
    }

    public interface IReportable : IIdentifiable
    {
        string GetReport();
    }

    public interface ITaskPerformer
    {
        void PerformTask(ConstructionTask task);
    }

    public interface ITeamManager
    {
        void AddTeamMember(Worker worker);
        void ShowTeam();
    }

    public interface IWarehouseRepository
    {
        void AddMaterial(Material material);
        Material? GetMaterial(string name);
        void ShowAllMaterials();
    }

    public interface ITaskQueue
    {
        void EnqueueTask(ConstructionTask task);
        ConstructionTask? DequeueTask();
    }

    public abstract class Person : IIdentifiable
    {
        public int Id { get; protected set; }
        public string FullName { get; protected set; }

        protected Person(int id, string fullName)
        {
            Id = id;
            FullName = fullName;
        }

        public virtual void ShowInfo()
        {
            Console.WriteLine($"[{Id}] {FullName}");
        }
    }

    public class Worker : Person, ITaskPerformer
    {
        public string Specialization { get; set; }

        public Worker(int id, string fullName, string specialization)
            : base(id, fullName)
        {
            Specialization = specialization;
        }

        public virtual void PerformTask(ConstructionTask task)
        {
            Console.WriteLine($"{FullName} виконує завдання: {task.Title}");
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"Робітник: {FullName}, спеціалізація: {Specialization}");
        }
    }

    public class Engineer : Worker, IReportable
    {
        public string Qualification { get; set; }

        public Engineer(int id, string fullName, string specialization, string qualification)
            : base(id, fullName, specialization)
        {
            Qualification = qualification;
        }

        public string GetReport()
        {
            return $"Інженер {FullName}, кваліфікація: {Qualification}";
        }

        public override void PerformTask(ConstructionTask task)
        {
            Console.WriteLine($"Інженер {FullName} контролює виконання завдання: {task.Title}");
        }
    }

    public class Foreman : Worker, ITeamManager
    {
        private List<Worker> _team = new List<Worker>();

        public Foreman(int id, string fullName, string specialization)
            : base(id, fullName, specialization)
        {
        }

        public void AddTeamMember(Worker worker)
        {
            _team.Add(worker);
        }

        public void ShowTeam()
        {
            Console.WriteLine($"Бригада майстра {FullName}:");
            foreach (var worker in _team)
            {
                Console.WriteLine($"- {worker.FullName}");
            }
        }

        public override void PerformTask(ConstructionTask task)
        {
            Console.WriteLine($"Майстер {FullName} розподіляє завдання: {task.Title}");
        }
    }

    public class ConstructionTask : IIdentifiable
    {
        public int Id { get; private set; }
        public string Title { get; set; }
        public int Priority { get; set; }

        public ConstructionTask(int id, string title, int priority)
        {
            Id = id;
            Title = title;
            Priority = priority;
        }

        public override string ToString()
        {
            return $"Завдання #{Id}: {Title}, пріоритет: {Priority}";
        }
    }

    public class Material : IIdentifiable
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public int Quantity { get; set; }

        public Material(int id, string name, int quantity)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"{Name} ({Quantity} од.)";
        }
    }

    public abstract class ConstructionMachine : IIdentifiable
    {
        public int Id { get; protected set; }
        public string Model { get; protected set; }

        protected ConstructionMachine(int id, string model)
        {
            Id = id;
            Model = model;
        }

        public virtual void Start()
        {
            Console.WriteLine($"Машина {Model} запущена");
        }
    }

    public class Crane : ConstructionMachine
    {
        public Crane(int id, string model) : base(id, model) { }

        public override void Start()
        {
            Console.WriteLine($"Кран {Model} почав підйом вантажу");
        }
    }

    public class Bulldozer : ConstructionMachine
    {
        public Bulldozer(int id, string model) : base(id, model) { }

        public override void Start()
        {
            Console.WriteLine($"Бульдозер {Model} почав розчищення майданчика");
        }
    }

    public class Warehouse : IWarehouseRepository
    {
        private Dictionary<string, Material> _materials = new Dictionary<string, Material>();

        public void AddMaterial(Material material)
        {
            _materials[material.Name] = material;
        }

        public Material? GetMaterial(string name)
        {
            return _materials.ContainsKey(name) ? _materials[name] : null;
        }

        public void ShowAllMaterials()
        {
            Console.WriteLine("Матеріали на складі:");
            foreach (var material in _materials.Values)
            {
                Console.WriteLine($"- {material}");
            }
        }
    }

    public class TaskQueueService : ITaskQueue
    {
        private Queue<ConstructionTask> _tasks = new Queue<ConstructionTask>();

        public void EnqueueTask(ConstructionTask task)
        {
            _tasks.Enqueue(task);
        }

        public ConstructionTask? DequeueTask()
        {
            if (_tasks.Count == 0) return null;
            return _tasks.Dequeue();
        }
    }

    public class Project
    {
        public string Name { get; set; }

        public string[] Zones { get; set; }

        public List<Worker> Workers { get; private set; } = new List<Worker>();

        public List<ConstructionMachine> Machines { get; private set; } = new List<ConstructionMachine>();

        public Stack<string> ActionHistory { get; private set; } = new Stack<string>();

        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ITaskQueue _taskQueue;

        public Project(string name, string[] zones, IWarehouseRepository warehouseRepository, ITaskQueue taskQueue)
        {
            Name = name;
            Zones = zones;
            _warehouseRepository = warehouseRepository;
            _taskQueue = taskQueue;
        }

        public void AddWorker(Worker worker)
        {
            Workers.Add(worker);
            ActionHistory.Push($"Додано працівника: {worker.FullName}");
        }

        public void AddMachine(ConstructionMachine machine)
        {
            Machines.Add(machine);
            ActionHistory.Push($"Додано техніку: {machine.Model}");
        }

        public void AddMaterial(Material material)
        {
            _warehouseRepository.AddMaterial(material);
            ActionHistory.Push($"Додано матеріал: {material.Name}");
        }

        public void AddTask(ConstructionTask task)
        {
            _taskQueue.EnqueueTask(task);
            ActionHistory.Push($"Додано завдання: {task.Title}");
        }

        public void ExecuteNextTask(ITaskPerformer performer)
        {
            var task = _taskQueue.DequeueTask();
            if (task == null)
            {
                Console.WriteLine("Черга завдань порожня");
                return;
            }

            performer.PerformTask(task);
            ActionHistory.Push($"Виконано завдання: {task.Title}");
        }

        public void ShowHistory()
        {
            Console.WriteLine("Історія дій:");
            foreach (var item in ActionHistory)
            {
                Console.WriteLine($"- {item}");
            }
        }
    }

    public class ReportService
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public ReportService(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public void ShowWarehouseReport()
        {
            Console.WriteLine("Звіт по складу:");
            _warehouseRepository.ShowAllMaterials();
        }
    }

    class Program
    {
        static void Main()
        {
            IWarehouseRepository warehouse = new Warehouse();
            ITaskQueue taskQueue = new TaskQueueService();

            string[] zones = { "Фундамент", "Стіни", "Дах" };

            Project project = new Project("Житловий комплекс", zones, warehouse, taskQueue);

            Worker worker1 = new Worker(1, "Іван Петренко", "Муляр");
            Engineer engineer = new Engineer(2, "Олег Коваль", "Інженер-будівельник", "Senior");
            Foreman foreman = new Foreman(3, "Сергій Бондар", "Майстер");

            foreman.AddTeamMember(worker1);
            foreman.AddTeamMember(engineer);

            project.AddWorker(worker1);
            project.AddWorker(engineer);
            project.AddWorker(foreman);

            project.AddMachine(new Crane(1, "Liebherr LTM"));
            project.AddMachine(new Bulldozer(2, "Caterpillar D6"));

            project.AddMaterial(new Material(1, "Цемент", 100));
            project.AddMaterial(new Material(2, "Цегла", 500));

            project.AddTask(new ConstructionTask(1, "Залити фундамент", 1));
            project.AddTask(new ConstructionTask(2, "Побудувати стіни", 2));

            foreman.ShowTeam();

            project.ExecuteNextTask(worker1);
            project.ExecuteNextTask(engineer);

            ReportService reportService = new ReportService(warehouse);
            reportService.ShowWarehouseReport();

            project.ShowHistory();
        }
    }
}