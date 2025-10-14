# 🏥 Hospital Management System

Console application built with **C# (.NET 8)** to manage **patients**, **doctors**, and **medical appointments**, including validation, logging, and simulated email sending.  
Developed as a **performance test / layered architecture practice project**.

---

## 🚀 Main Features

- 👨‍⚕️ **Doctor Management**
  - Register, edit, delete, and list doctors.
  - Filter doctors by specialty.
- 🧍‍♂️ **Patient Management**
  - Register, edit, delete, and list patients.
  - Validate duplicates by document number.
- 📅 **Appointment Management**
  - Schedule, cancel, mark as attended, and delete appointments.
  - List appointments by patient or doctor.
  - Automatically detect schedule conflicts.
- ✉️ **Email Sending Simulation**
  - Simulates email sending through console logs, with history saved in `EmailHistory`.
- 🔍 **Robust Validations**
  - Names, documents, ages, emails, and phone numbers validated with regular expressions.
  - Interactive input retries through `InputHelper`.
- 💬 **Integrated Logging**
  - Uses `ILogger` for operation and error logging.
- 🧱 **Layered Architecture**
  - Clear separation between Models, Repositories, Services, Controllers, and Utilities.

---

## 🧠 Implemented Validations

The `Validator` module enforces:

| Data Type | Validation Rule |
|------------|----------------|
| **Name** | Only letters and spaces, between 2–100 characters |
| **Document** | Unique, 4–20 alphanumeric characters |
| **Age** | Between 0 and 120 years |
| **Email** | Valid format `user@domain.com` |
| **Phone** | 7–10 digits, normalized and truncated if it exceeds 10 digits |

---

## 🧰 Requirements

- 🟦 [.NET 8 SDK](https://dotnet.microsoft.com/download)
- 🧠 Visual Studio, VS Code, or JetBrains Rider

---

## ⚙️ How to Run the Project

1. **Restore dependencies and build:**

  ```bash
  dotnet restore
  dotnet build
  Run the application:
  dotnet run

You will see an interactive console menu:

    ╔════════════════════════════════════╗
    ║   HOSPITAL MANAGEMENT SYSTEM       ║
    ╠════════════════════════════════════╣
    ║ 1. Patient Management              ║
    ║ 2. Doctor Management               ║
    ║ 3. Appointment Management          ║
    ║ 4. Email History                   ║
    ║ 0. Exit                            ║
    ╚════════════════════════════════════╝


## 🧪 Recommended Test Scenarios

  Register a valid patient

  Name: Juan Pérez

  Document: 12345678

  Age: 30

  Phone: +57 3123456789 → normalized and truncated to 10 digits.

  Email: juan.perez@example.com

  Try registering a patient with a duplicate document → should show an error.

  Register a doctor and validate uniqueness by name + specialty.

  Schedule two appointments for the same doctor and time → the second attempt should fail (conflict).

  Schedule two appointments for the same patient and time → should also fail.

  Schedule a valid appointment → triggers simulated email sending and appears in email history.

  Edit a patient or doctor with invalid data → should reject or prompt for retry.

## 🧩 Technologies Used
  Component	Technology
  Language	C#
  Framework	.NET 8
  Architecture	Layered MVC (Controllers / Services / Repositories / Models / Utils)
  Logging	Microsoft.Extensions.Logging
  Persistence	In-memory Lists (List<T>) using LINQ
  Validation	System.Text.RegularExpressions

## 👨‍💻 Author
Developed by: Allison Pauleth Segrera Pardo
📧 Email: paulethsegrera@gmail.com
🦎 Clan: Caimán
🪪 ID: 1043447980
💻 GitHub: Pauleth21
