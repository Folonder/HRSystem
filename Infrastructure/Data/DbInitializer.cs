using Domain.Entities;

namespace Infrastructure.Data;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (!context.Database.EnsureCreated())
        {
            return;
        }

        // Создаем подразделения
        var departments = new List<Department>
        {
            new() { Name = "Подразделение информационных технологий" },
            new() { Name = "Отдел продаж" },
            new() { Name = "Отдел маркетинга" },
            new() { Name = "Финансовый отдел" },
            new() { Name = "Отдел кадров" },
            new() { Name = "Производственное подразделение" },
            new() { Name = "Отдел исследований и разработок" },
            new() { Name = "Отдел клиентской поддержки" },
            new() { Name = "Отдел контроля качества" },
            new() { Name = "Юридический отдел" }
        };

        context.Departments.AddRange(departments);
        context.SaveChanges();

        // Задаем иерархию между подразделениями
        departments[0].SubDepartments = new List<Department>
        {
            departments[1],
            departments[2],
            departments[5]
        };

        departments[1].SubDepartments = new List<Department>
        {
            departments[7],
            departments[8]
        };

        departments[2].SubDepartments = new List<Department>
        {
            departments[6]
        };

        departments[4].SubDepartments = new List<Department>
        {
            departments[9]
        };

        context.SaveChanges();

        // Создаем истории подразделений
        var departmentsHistories = new List<DepartmentHistory>
        {
            new ()
                { DepartmentId = departments[0].Id, OpenDate = new DateTime(2020, 1, 1), CloseDate = null },
            new ()
                { DepartmentId = departments[1].Id, OpenDate = new DateTime(2020, 1, 1), CloseDate = null },
            new ()
                { DepartmentId = departments[2].Id, OpenDate = new DateTime(2020, 1, 1), CloseDate = new DateTime(2023, 1, 1) },
            new ()
            {
                DepartmentId = departments[3].Id, OpenDate = new DateTime(2020, 1, 1),
                CloseDate = new DateTime(2022, 1, 1)
            },
            new ()
            {
                DepartmentId = departments[4].Id, OpenDate = new DateTime(2020, 1, 1),
                CloseDate = new DateTime(2022, 2, 1)
            },
            new ()
                { DepartmentId = departments[5].Id, OpenDate = new DateTime(2020, 1, 1), CloseDate = null },
            new ()
                { DepartmentId = departments[6].Id, OpenDate = new DateTime(2020, 1, 1), CloseDate = new DateTime(2023, 1, 1) },
            new ()
                { DepartmentId = departments[7].Id, OpenDate = new DateTime(2020, 1, 1), CloseDate = null },
            new ()
                { DepartmentId = departments[8].Id, OpenDate = new DateTime(2020, 1, 1), CloseDate = null },
            new ()
                { DepartmentId = departments[9].Id, OpenDate = new DateTime(2020, 1, 1), CloseDate = new DateTime(2022, 2, 1) }
        };

        context.DepartmentsHistories.AddRange(departmentsHistories);
        context.SaveChanges();

        // Создаем сотрудников
        var employees = new List<Employee>
        {
            new () {Name = "Иванов Иван Иванович"},
            new () {Name = "Петров Пётр Петрович"},
            new () {Name = "Сидоров Сергей Сергеевич"},
            new () {Name = "Смирнова Анна Викторовна"},
            new () {Name = "Васильева Елена Дмитриевна"},
            new () {Name = "Фёдоров Алексей Алексеевич"},
            new () {Name = "Михайлова Мария Александровна"},
            new () {Name = "Кузнецов Дмитрий Николаевич"},
            new () {Name = "Павлова Ольга Владимировна"},
            new () {Name = "Николаев Артём Евгеньевич"}
        };


        context.Employees.AddRange(employees);
        context.SaveChanges();

        // Создаем истории сотрудников
        var employeesHistories = new List<EmployeeHistory>
        {
            new ()
            {
                EmployeeId = employees[0].Id, DepartmentId = departments[0].Id, HireDate = new DateTime(2020, 1, 1),
                FireDate = new DateTime(2022, 1, 1)
            },
            new ()
            {
                EmployeeId = employees[1].Id, DepartmentId = departments[1].Id, HireDate = new DateTime(2020, 1, 1),
                FireDate = new DateTime(2022, 1, 1)
            },
            new ()
            {
                EmployeeId = employees[2].Id, DepartmentId = departments[2].Id, HireDate = new DateTime(2020, 1, 1),
                FireDate = new DateTime(2022, 1, 1)
            },
            new ()
            {
                EmployeeId = employees[3].Id, DepartmentId = departments[3].Id, HireDate = new DateTime(2020, 1, 1),
                FireDate = new DateTime(2022, 1, 1)
            },
            new ()
            {
                EmployeeId = employees[4].Id, DepartmentId = departments[4].Id, HireDate = new DateTime(2020, 1, 1),
                FireDate = new DateTime(2022, 1, 1)
            },
            new ()
            {
                EmployeeId = employees[5].Id, DepartmentId = departments[5].Id, HireDate = new DateTime(2020, 1, 1),
                FireDate = new DateTime(2022, 1, 1)
            },
            new ()
            {
                EmployeeId = employees[6].Id, DepartmentId = departments[6].Id, HireDate = new DateTime(2020, 1, 1),
                FireDate = new DateTime(2022, 1, 1)
            },
            new ()
            {
                EmployeeId = employees[7].Id, DepartmentId = departments[7].Id, HireDate = new DateTime(2020, 1, 1),
                FireDate = new DateTime(2022, 1, 1)
            },
            new ()
            {
                EmployeeId = employees[8].Id, DepartmentId = departments[8].Id, HireDate = new DateTime(2020, 1, 1),
                FireDate = null
            },
            new ()
            {
                EmployeeId = employees[9].Id, DepartmentId = departments[9].Id, HireDate = new DateTime(2020, 1, 1),
                FireDate = null
            }
        };

        context.EmployeesHistories.AddRange(employeesHistories);
        context.SaveChanges();

        var newEmployeesHistories = new List<EmployeeHistory>
        {
            new ()
            {
                EmployeeId = employees[0].Id, DepartmentId = departments[0].Id, HireDate = new DateTime(2022, 1, 2),
                FireDate = null
            },
            new ()
            {
                EmployeeId = employees[1].Id, DepartmentId = departments[1].Id, HireDate = new DateTime(2022, 1, 2),
                FireDate = null
            },
            new ()
            {
                EmployeeId = employees[2].Id, DepartmentId = departments[2].Id, HireDate = new DateTime(2022, 1, 2),
                FireDate = null
            },
            new ()
            {
                EmployeeId = employees[3].Id, DepartmentId = departments[1].Id, HireDate = new DateTime(2022, 1, 2),
                FireDate = null
            },
            new ()
            {
                EmployeeId = employees[4].Id, DepartmentId = departments[2].Id, HireDate = new DateTime(2022, 1, 2),
                FireDate = null
            },
            new ()
            {
                EmployeeId = employees[5].Id, DepartmentId = departments[5].Id, HireDate = new DateTime(2022, 1, 2),
                FireDate = null
            },
            new ()
            {
                EmployeeId = employees[6].Id, DepartmentId = departments[6].Id, HireDate = new DateTime(2022, 1, 2),
                FireDate = null
            },
            new ()
            {
                EmployeeId = employees[7].Id, DepartmentId = departments[7].Id, HireDate = new DateTime(2022, 1, 2),
                FireDate = null
            }
        };
        
        context.EmployeesHistories.AddRange(newEmployeesHistories);
        context.SaveChanges();
    }
}