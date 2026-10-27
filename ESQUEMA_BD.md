# Esquema de la base `BarrioCerradoII`

Relevado el 2026-10-20 desde `26.216.108.167`. Regenerable con `sqlcmd` (ver al final).

## Tablas y columnas

### Persona
| Columna | Tipo | Notas |
|---|---|---|
| IDPersona | int | PK, IDENTITY |
| Nombre | varchar(100) | nullable |
| Apellido | varchar(100) | nullable |
| DNI | varchar(20) | nullable |
| FechaAlta | datetime | NOT NULL |
| HabilitadoReconocimiento | bit | nullable |
| FK_IDCategoria | int | → CategoriaPersona |
| FK_Estado | int | → Estado |
| Password | varchar(100) | nullable |

### UsuarioSistema
| Columna | Tipo | Notas |
|---|---|---|
| IDUsuario | int | PK, IDENTITY |
| Username | varchar(50) | NOT NULL |
| PasswordHash | varchar(255) | NOT NULL |
| NombreCompleto | varchar(100) | NOT NULL |
| FK_Estado | int | NOT NULL → Estado |
| FK_Rol | int | NOT NULL → Rol |
| Dni | nchar(20) | NOT NULL |
| Foto | varchar(MAX) | nullable |

### Lote
| Columna | Tipo | Notas |
|---|---|---|
| IDLote | int | PK, IDENTITY |
| LoteNum | varchar(20) | NOT NULL |
| DireccionInterna | varchar(150) | nullable |
| Latitud | decimal | nullable |
| Longitud | decimal | nullable |
| Observaciones | varchar(MAX) | nullable |

### Residencia
Vincula personas con lotes (histórico, con vigencia).

| Columna | Tipo | Notas |
|---|---|---|
| IDResidencia | int | PK, IDENTITY |
| id_Persona | int | NOT NULL → Persona |
| id_Lote | int | NOT NULL → Lote |
| Fecha_Desde | date | NOT NULL |
| Fecha_Hasta | date | nullable (NULL = vigente) |
| Tipo_Vinculo | varchar(50) | NOT NULL |

### Vehiculo
| Columna | Tipo | Notas |
|---|---|---|
| id_vehiculo | int | PK, IDENTITY |
| Patente | varchar(15) | NOT NULL |
| Seguro | varchar(100) | NOT NULL |
| vencimiento_seguro | date | NOT NULL |

### Persona_Vehiculo
Tabla puente N:N.

| Columna | Tipo | Notas |
|---|---|---|
| IDPersona | int | NOT NULL → Persona |
| id_vehiculo | int | NOT NULL → Vehiculo |

### Movimiento
Registro de entradas/salidas.

| Columna | Tipo | Notas |
|---|---|---|
| IDMovimiento | int | PK, IDENTITY |
| FechaHora | datetime | NOT NULL |
| TipoMovimiento | varchar(50) | NOT NULL ("Ingreso"/"Egreso") |
| Autorizado | bit | NOT NULL |
| Detalle | varchar(255) | nullable |
| FK_IDPersona | int | NOT NULL → Persona |
| FK_IDVehiculo | int | nullable → Vehiculo |
| FK_IDLote | int | nullable → Lote |
| FK_IDPuntoAcceso | int | NOT NULL → PuntoAcceso |

### PreAcreditacion
Autorización anticipada de visitas/proveedores.

| Columna | Tipo | Notas |
|---|---|---|
| IDPreacreditacion | int | PK, IDENTITY |
| DNI | varchar(20) | NOT NULL |
| Nombre | varchar(100) | NOT NULL |
| Apellido | varchar(100) | NOT NULL |
| FK_IDCategoria | int | nullable → CategoriaPersona |
| FK_IDLote | int | NOT NULL → Lote |
| FK_IDResidenteAutoriza | int | NOT NULL → Persona |
| Fecha_Desde | date | NOT NULL |
| Fecha_Hasta | date | NOT NULL |
| Motivo | varchar(250) | NOT NULL |
| Estado | varchar(20) | NOT NULL (texto libre, **no** FK a `Estado`) |

### RestriccionHoraria
| Columna | Tipo | Notas |
|---|---|---|
| ID_RestriccionHoraria | int | PK, IDENTITY |
| Vigente_Desde | date | NOT NULL |
| Vigente_Hasta | date | NOT NULL |
| Dia_Semana | varchar(20) | NOT NULL |
| Hora_Desde | time | NOT NULL |
| Hora_Hasta | time | NOT NULL |
| FK_ID_Categoria | int | nullable → CategoriaPersona |

### PuntoAcceso
| Columna | Tipo | Notas |
|---|---|---|
| IDPuntoAcceso | int | PK (**no** IDENTITY: hay que asignar el ID a mano) |
| Ubicacion | varchar(50) | NOT NULL |
| Tipo_Acceso | varchar(50) | NOT NULL |
| EsBiometrico | bit | NOT NULL |

### Bitacora
| Columna | Tipo | Notas |
|---|---|---|
| IDBitacora | int | PK, IDENTITY |
| FechaHora | datetime | NOT NULL |
| Accion | varchar(50) | NOT NULL |
| TablaAfectada | varchar(50) | NOT NULL |
| IDRegistroAfectado | varchar(50) | nullable |
| ValorAnterior | varchar(MAX) | nullable |
| ValorNuevo | varchar(MAX) | nullable |
| FK_IDUsuario | int | NOT NULL → UsuarioSistema |

### Tablas de referencia

**CategoriaPersona** (`IDCategoria` PK IDENTITY, `Descripcion` varchar(100))

| ID | Descripción |
|---|---|
| 1 | Residente |
| 2 | Propietario / Inquilino |
| 3 | Visita |
| 4 | Proveedor / Empleado |
| 5 | Proveedor |

**Estado** (`ID_Estado` PK IDENTITY, `Descripcion` varchar(100))

| ID | Descripción |
|---|---|
| 1 | Activo |
| 2 | Inactivo |
| 3 | Pendiente |

**Rol** (`ID_Rol` PK IDENTITY, `Descripcion` varchar(100), `Permisos` varchar(255))

| ID | Descripción |
|---|---|
| 1 | Residente |
| 2 | Propietario |
| 3 | Administrador |
| 4 | Visita |
| 5 | Proveedor |

**PuntoAcceso**

| ID | Ubicación | Tipo |
|---|---|---|
| 1 | Guardia Principal | Peatonal/Vehicular |
| 2 | Acceso Proveedores | Vehicular |

## Foreign keys

```
Bitacora.FK_IDUsuario               -> UsuarioSistema.IDUsuario
Movimiento.FK_IDPersona             -> Persona.IDPersona
Movimiento.FK_IDVehiculo            -> Vehiculo.id_vehiculo
Movimiento.FK_IDLote                -> Lote.IDLote
Movimiento.FK_IDPuntoAcceso         -> PuntoAcceso.IDPuntoAcceso
Persona.FK_IDCategoria              -> CategoriaPersona.IDCategoria
Persona.FK_Estado                   -> Estado.ID_Estado
Persona_Vehiculo.IDPersona          -> Persona.IDPersona
Persona_Vehiculo.id_vehiculo        -> Vehiculo.id_vehiculo
PreAcreditacion.FK_IDCategoria      -> CategoriaPersona.IDCategoria
PreAcreditacion.FK_IDLote           -> Lote.IDLote
PreAcreditacion.FK_IDResidenteAutoriza -> Persona.IDPersona
Residencia.id_Persona               -> Persona.IDPersona
Residencia.id_Lote                  -> Lote.IDLote
RestriccionHoraria.FK_ID_Categoria  -> CategoriaPersona.IDCategoria
UsuarioSistema.FK_Estado            -> Estado.ID_Estado
UsuarioSistema.FK_Rol               -> Rol.ID_Rol
```

## Detalles a tener en cuenta al escribir queries

- **Nomenclatura inconsistente.** Conviven tres estilos: `IDPersona`, `id_vehiculo`, `ID_Estado`. Hay que mirar la tabla antes de escribir el JOIN.
- `Persona.DNI` es `varchar(20)` y es nullable; no hay unique constraint declarada, así que puede haber duplicados.
- `PreAcreditacion.Estado` es texto libre, no apunta a la tabla `Estado`.
- `PuntoAcceso.IDPuntoAcceso` no es IDENTITY: los INSERT deben traer el ID explícito.
- `Persona` tiene `Password` y `UsuarioSistema` tiene `PasswordHash` — son dos mecanismos de login distintos conviviendo.
- `CategoriaPersona` tiene 4 y 5 solapadas ("Proveedor / Empleado" y "Proveedor").
- `Residencia.Fecha_Hasta` NULL significa vínculo vigente.

## Regenerar este documento

```bash
sqlcmd -S 26.216.108.167 -d BarrioCerradoII -U Grupo07 -P '<password>' -h -1 -W -Q "SELECT t.name, c.name, ty.name FROM sys.columns c JOIN sys.tables t ON t.object_id=c.object_id JOIN sys.types ty ON ty.user_type_id=c.user_type_id ORDER BY t.name, c.column_id;"
```
