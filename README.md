# ShareSquare: A Hub for Book & Game CD Enthusiasts

Welcome to ShareSquare, an innovative platform crafted for book lovers and gamers. Developed with ASP.NET Core MVC and Bootstrap, the project thrives on SQL-backed storage and SignalR-fueled real-time communication. Designed around the N-Tier Architecture, we seek to promote a community where users can seamlessly borrow, buy, or sell books and game CDs.

## Features

- **User Management**:
    - **User Registration**: Simplified registration process for new users.
    - **Login System**: Secure authentication mechanism.
    - **User Authorization**: Protected resources to ensure security.
    - **Password Management**: Features like 'Forgot Email', 'Reset Password', and 'Confirm Email'.
    - **External Authentication**: Users can login using Facebook, Gmail.
    - **Two-Factor Authentication**: Enhanced security with QR code scanning or input string verification.

- **Community Interaction**:
    - **Real-Time Chat**: Facilitated by SignalR, enabling users to communicate instantaneously.
    - **Notifications**: Users are alerted of new messages or interactions.

- **Item Management**:
    - **Add Items**: Users can add books or game CDs, including image uploads.
    - **Browse Items**: Seamless navigation through available books and CDs.

📖 _Note: For a first-time setup, make sure to configure the Secret Manager for user secrets._

## Getting Started

### Prerequisites

- .NET Core 6 or later
- SQL Server (For database management)
- Any IDE that supports ASP.NET Core (e.g., Visual Studio)

### Setting up the Project

1. Clone the repository from GitHub:
    ```bash
    git clone https://github.com/SDDP-Professional-2023/ShareSquareApp
    ```
    _Press enter to start the clone process._

2. Navigate to the cloned repository:
    ```bash
    cd path_to_cloned_repo
    ```

3. (Optional) Open the project in your preferred IDE.

4. Set up the database connection string in `appsettings.json` or via user secrets.

5. Build and run the application:
    ```bash
    dotnet build
    dotnet run
    ```

## Contributing to the Project

We're always open to collaboration! If you're looking to contribute:

1. Navigate to the main branch:
    ```bash
    git checkout main
    ```

2. Sync with the latest updates:
    ```bash
    git pull origin main
    ```

3. Create a new branch for your changes:
    ```bash
    git checkout -b branch_name
    ```

4. Commit and push your changes:
    ```bash
    git add .
    git commit -m "Your meaningful commit message"
    git push origin branch_name
    ```

Ensure your contributions align with the existing code structure and standards. It's beneficial to check open issues related to your changes before contributing.

## License

ShareSquare operates under the MIT License. Dive into the `LICENSE` file for a comprehensive understanding.

## Acknowledgments

- Immense gratitude to all our dedicated contributors and vibrant user community.

