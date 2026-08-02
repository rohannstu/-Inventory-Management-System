namespace InventoryManagementSystem.Domain.Enums;

//N:B: Adding a new role directly without numbering like 
//below will create a problem of data loss of old DB.
//Rather we can do numbering which will help to not loss data

//Rule of numbering: number 0 the most least privileged role 
//like here Staff so if a bug happens it woun't create big problem 
//in the business or sytem
public enum UserRole
{
    Staff = 0,
    Manager = 1,
    Admin = 2 // Here admin is numbered 2 because it is most important
}