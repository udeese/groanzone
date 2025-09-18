# The Groan Zone

Full-stack ASP.NET Core MVC app where users post dad jokes and rate them (1–4).

## Tech
- ASP.NET Core MVC
- EF Core + Pomelo (MariaDB)
- Session auth, BCrypt password hashing
- CSRF protection, async queries, AsNoTracking for reads

## Run locally
1. Create DB & user:
   ```sql
   CREATE DATABASE groanzone_db CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
   CREATE USER 'groanzone'@'localhost' IDENTIFIED BY 'SuperSecretPassword!';
   GRANT ALL PRIVILEGES ON groanzone_db.* TO 'groanzone'@'localhost';
   FLUSH PRIVILEGES;
