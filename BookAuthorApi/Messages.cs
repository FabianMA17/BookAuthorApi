namespace BookAuthorApi;
public static class Messages
{
    // Auth messages
    public const string InvalidUsernameOrPassword = "Usuario o contraseña inválidos";
    public const string LoginError = "Ocurrió un error durante el inicio de sesión";

    // General messages
    public const string AuthorNotFound = "Autor no encontrado";
    public const string BookNotFound = "Libro no encontrado";
    public const string NoValidFileProvided = "No se proporcionó un archivo válido";
    public const string FileMustBeCsv = "El archivo debe tener extensión .csv";
    public const string BulkUploadCompleted = "Carga masiva completada";
}