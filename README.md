# AVL_Ruben_Ibañez

Sistema de Gestión de Expedientes Médicos — Hospital San Gabriel
Laboratorio 3: Árbol AVL en C# — Procesos y Algoritmos II

## Requisitos
- .NET SDK 10.0 o superior (https://dotnet.microsoft.com/download)

## Ejecución

```bash
dotnet run
```

## Estructura del proyecto

```
AVL_Ruben_Ibañez/
├── Program.cs                 Menú principal y submenús
├── Modelos/
│   └── Expediente.cs           Modelo de datos del paciente
├── Estructuras/
│   ├── NodoAVL.cs               Nodo del árbol
│   └── ArbolAVL.cs               Implementación propia del Árbol AVL
├── Servicios/
│   └── GestorArchivos.cs         Guardar / importar CSV
└── Utilidades/
    └── Validaciones.cs            Lectura y validación de entradas
```

## Notas
- Spectre.Console se usa únicamente con fines estéticos
- Al guardar datos se crea **expedientes.csv** en la carpeta
  del ejecutable. Al volver a ejecutar el programa, si ese archivo existe, se
  ofrece importarlo automáticamente.
