# 📚 BookAuthorApi

API Web desarrollada con ASP.NET Core para la gestión de libros y autores. Implementa **Clean Architecture**, utiliza **Entity Framework Core** como ORM y **SQLite** como base de datos.

---

## 🚀 Características

- Gestión de libros y autores
- Arquitectura limpia (Clean Architecture)
- Persistencia con Entity Framework Core
- Base de datos SQLite
- Autenticación mediante JWT
- Contenerización con Docker

---

## 📋 Prerrequisitos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

- SDK de .NET 10
- Docker (Docker Desktop en Windows/Mac o Docker nativo en Linux)

---

## 📌 Notas adicionales

- La aplicación utiliza SQLite, por lo que la base de datos está incluida dentro del contenedor Docker.
- Se requiere autenticación mediante JWT para acceder a los endpoints protegidos.

---

## 👤 Usuarios de prueba

Para facilitar las pruebas de autenticación, puedes utilizar los siguientes usuarios:

| Usuario | Contraseña |
|---------|------------|
| user1   | password1  |
| user2   | password2  |
| user3   | password3  |

> ⚠️ Estos usuarios son solo para pruebas en entorno local.

---

## 🔑 Obtener token JWT

Para acceder a los endpoints protegidos:

1. Realiza una petición al endpoint de autenticación (por ejemplo `/api/auth/login`)
2. Envía las credenciales de uno de los usuarios de prueba
3. Recibirás un token JWT
4. Incluye el token en las peticiones usando el header:

```http
Authorization: Bearer {tu_token}
