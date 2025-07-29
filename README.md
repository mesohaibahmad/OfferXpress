# OfferXpress

**OfferXpress** is a web-based offer generation and management tool developed for retail companies operating across multiple branches and countries. It allows sales personnel to enter customer and offer data through a user-friendly web form and generate multilingual PDF offers based on predefined templates.

## Table of Contents

- [Project Features](#project-features)
- [Technology Stack](#technology-stack)
- [System Requirements](#system-requirements)
- [Installation & Deployment](#installation--deployment)
- [Database Structure](#database-structure)
- [PDF Generation Rules](#pdf-generation-rules)
- [Language Support](#language-support)
- [Security and Data Integrity](#security-and-data-integrity)
- [License](#license)

---

## Project Features

- Responsive, form-based UI closely resembling final offer layout.
- Multilingual support: English, German, Slovenian, Italian.
- Dynamic PDF offer generation (based on filled-in form).
- Role-based interaction with generated offers (edit/copy restrictions).
- Strict data validation and formatting:
  - Date format: `dd.MM.yyyy`
  - Quantity: `1,00`
  - Amount: `1.234,00 €`
- Automatic calculations for VAT, totals, and exclusions.
- Offers are immutable once PDF is generated.
- List of generated offers from the last 3 days per country.
- Copy-to-edit functionality to maintain version history.

---

## Technology Stack

- **Frontend**: HTML, CSS, JavaScript (Bootstrap, optionally React or jQuery)
- **Backend**: ASP.NET (C#)
- **Database**: Microsoft SQL Server
- **PDF Generation**: iTextSharp or similar .NET library
- **Web Server**: IIS (Internet Information Services)

---

## System Requirements

- Windows Server with IIS Installed
- .NET Framework (v4.7.2 or higher) or .NET Core Runtime (if applicable)
- MSSQL Server (configured and running)
- Sufficient write permissions to a designated folder for PDF storage

---

## Installation & Deployment

1. **Database Setup**
   - Add migration with Entity-Framework

2. **App Configuration**
   - Update `web.config` or `appsettings.json` with the correct database connection string.
   - Set the file path for saving generated PDF documents.
   - Set localization settings if using resource files for translations.

3. **Testing**
   - Open the application in a browser
   - Fill the form, switch between languages, and test PDF generation.

---

## Database Structure


Main Tables:
- `Offers`
- `Offered_item_rows`
- `Branches`
- `Countries`

Important Fields:
- `Offers.PDF_Generated` (bool) — Controls immutability
- `Countries.Default_VAT` — Used as the default for product lines

---

## PDF Generation Rules

- PDF is generated **only** if all required fields are filled.
- Once generated:
  - `Offers.PDF_Generated` is set to `TRUE`
  - Offer becomes **read-only**
  - Cannot be deleted
- A "Copy and Edit" option allows creating a **new editable offer version**
- Templates are available in:
  - 🇸🇮 Slovenian
  - 🇩🇪 German
  - 🇬🇧 English
  - 🇮🇹 Italian

---

## Language Support

The user can switch between supported languages at any time before PDF generation. UI text, labels, and the final PDF template content adapt accordingly.

---

## Security and Data Integrity

- Server-side validation ensures all mandatory fields are filled.
- PDF generation logic prevents modification of finalized offers.
- Offer cloning feature retains full version history.
- Only offers from the last 3 days per country are listed under the "Offer Archive".

---

## License


This project is licensed under the [MIT License](LICENSE).

---

## Contact / Support

Please contact at sohaib.pak2017@gmail.com for issues related to deployment or use.
