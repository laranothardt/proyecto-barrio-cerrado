# Enunciado — Sistema de Barrio Cerrado

En un barrio cerrado se controla el ingreso y egreso de todas las personas y vehículos. Cada movimiento debe quedar registrado en el sistema para garantizar la seguridad.

Al momento de dar de alta a un residente, visitante frecuente o proveedor, se toma una foto de referencia que queda guardada en el sistema. Esa foto se utiliza para validar los ingresos posteriores, evitando tener que capturar una nueva cada vez.

En algunos casos, el barrio implementa reconocimiento facial, lo que permite que ciertas personas autorizadas ingresen directamente al detectar su rostro en la cámara de acceso. El sistema debe permitir gestionar quién tiene habilitado el ingreso por rostro y quién no.

Cada lote del barrio guarda su posición mediante coordenadas GPS, lo que permite indicar a los ingresantes la ubicación exacta a la que deben dirigirse. Por ejemplo, cuando Marta González ingresa como visitante, el sistema le muestra la ubicación del lote de su amiga para que pueda llegar sin inconvenientes.

El sistema también debe poder generar reportes de ingreso y egreso. Por ejemplo, al final del día se puede obtener un informe con todos los residentes que ingresaron y salieron, los visitantes autorizados, los proveedores que trabajaron en el barrio y los intentos de acceso denegados. Además, se deben poder generar distintos tipos de reportes: por persona, por categoría, por lote o por rango de fechas.

Para agilizar el acceso, el barrio maneja una lista de pre-acreditados. En esa lista se registran los datos básicos de las personas autorizadas: DNI, apellido y nombre. Por ejemplo, si Pedro Gómez está pre-acreditado como proveedor de jardinería, al llegar su ingreso se valida rápidamente con su DNI y foto de referencia.

## Requerimientos consolidados

- Registrar ingresos y egresos de personas y vehículos.
- Validar identidades con foto de referencia o reconocimiento facial.
- Identificar categoría de cada persona: propietario, visitante, proveedor, obrero.
- Aplicar restricciones según franjas horarias y días de la semana.
- Guardar la ubicación de cada lote con coordenadas GPS.
- Generar múltiples reportes de movimientos (por persona, categoría, lote, rango de fechas).
- Gestionar listas de pre-acreditados con datos básicos (DNI, apellido, nombre).
