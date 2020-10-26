# CompanyStructureApp
Company Structure App is a .NET Core onion architecture application with console interface.
It allows user to create hierarchical structure of the company in the form of a tree and perform various searches on this structure.
For example, root element is a director to whom the supply managers and sales managers are subordinated,
to the supply manager the workers of type X and Y are subordinated, and to the sales manager the workers of type A and B. 

Used design patterns: Strategy, Visitor, Composite, Factory Method

Main functionality:
• Find all employees whose salary is higher than the specified and the highest salary among all employees
• Find all employees who report directly to the task
• Find all employees in a given position
• Display the structure of the company in different formats:
  + by direct subordination 
  + by the height of the position in the company


Created for study purposes.

Created by DenKu1 (https://github.com/DenKu1)
