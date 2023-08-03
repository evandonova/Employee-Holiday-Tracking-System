# Employee Holiday Tracking System
This is an ASP.NET Core app for **tracking employees holiday requests**.

## Roles in the App
We have **three user roles** in the app - **admin**, **supervisor** and **employee**.

### Employee
 - Can log in the app with credentials, given by their supervisor.
 - Can view their profile information like names, remaining holiday days and supervisor's names.
 - Can create holiday requests.
 - Can view their created holiday requests (pending, approved and disapproved).

We have **one seeded employee** in the database with login credentials `employee@mail.com` - `employee123#`.

<kbd>
  <img src="https://github.com/evandonova/Employee-Holiday-Tracking-System/assets/69080997/425290d6-f7b0-491c-81e9-0560c947bacf" height="300">
</kbd>
<kbd>
  <img src="https://github.com/evandonova/Employee-Holiday-Tracking-System/assets/69080997/85aa107c-4d68-4c22-9795-c389ed91c073" height="300">
</kbd>

### Supervisor
 - Can log in the app with credentials, given by the admin.
 - Can see their profile information like names and added employees.
 - Can add new employees.
 - Can view information of employees they created.
 - Can edit employees they created.
 - Can delete employees they created (employee holiday requests are also deleted).
 - Can see pending requests from their employees.
 - Can approve and disapprove employees holiday requests.
   - When they want to approve a request, the employee that requested it should have enough holiday days remaining.
   - When they want to disapprove a request, they should provide a statement why.

We have **one seeded supervisor** in the database with login credentials `supervisor@mail.com` - `supervisor123#`.

<kbd>
  <img src="https://github.com/evandonova/Employee-Holiday-Tracking-System/assets/69080997/bb142254-b128-45cd-8fd3-577f8604ca89" width="500">
</kbd>

<kbd>
  <img src="https://github.com/evandonova/Employee-Holiday-Tracking-System/assets/69080997/2367c881-bc03-4b5f-be32-244f417ef037" height="250">
</kbd>


### Admin
 - The **admin is only one**, other admins cannot be created.
 - Admin can see their profile information like names and added supervisors.
 - Can add new supervisors.
 - Can view information of supervisors they created.
 - Can edit supervisors they created.
 - Can delete supervisors they created (their employees are also deleted).

Admin **login credentials**: `admin@mail.com` - `admin123#`.

<kbd>
  <img src="https://github.com/evandonova/Employee-Holiday-Tracking-System/assets/69080997/c19c0a64-3df8-44c4-b901-cdaa2b8b0594" height="300">
</kbd>

## Technologies Used
Include ASP.NET Core, Entity Framework Code, SQL Server, HTML, CSS, JavaScript.

## Database Schema
<kbd>
  <img src="https://github.com/evandonova/Employee-Holiday-Tracking-System/assets/69080997/c1b3b169-6a80-4d9d-a103-6195bc387c6a" height="300">
</kbd>


 
