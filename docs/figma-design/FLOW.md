# Flujo

1. Entrypoint: bienvenida.png
2. Al darle click iniciar sesión en bienvenida: inicio-sesion.png
3. Al ingresar credenciales: inicio-sesion-con-credenciales.png
4. Si el ID de la cuenta no esta bloqueada y las credenciales son correctas: perfil-usuario.png
5. Si despues de iniciar sesión el usuario se inactiva por más de 20 minutos: apunto-de-expirar-sesion.png
6. Si no logra extender sesion: inicio-sesion-despues-de-sesion-expirada.png
7. Si extendio la sesión: perfil-usuario-despues-extender-sesion.png
8. Si el ID de la cuenta no esta bloqueada y las credenciales son incorrectas y no ha superado el máximo de 5 reintentos: inicio-sesion-credenciales-incorrectas.png
9. Si el ID de la cuenta está bloqueada o las credenciales son incorrectas y supero el máximo de 5 reintentos: bloqueo-de-sesion.png

## Diagrama

```mermaid
graph TD
    %% Nodos principales
    A[bienvenida.png]
    B[inicio-sesion.png]
    C[inicio-sesion-con-credenciales.png]
    D[perfil-usuario.png]
    E[inicio-sesion-credenciales-incorrectas.png]
    F[bloqueo-de-sesion.png]
    G[apunto-de-expirar-sesion.png]
    H[inicio-sesion-despues-de-sesion-expirada.png]
    I[perfil-usuario-despues-extender-sesion.png]

    %% Flujo de inicio de sesión
    A -->|Click en iniciar sesión| B
    B -->|Ingresar credenciales| C

    %% Validaciones de credenciales y estado de cuenta
    C -->|ID no bloqueado y credenciales correctas| D
    C -->|ID no bloqueado, cred. incorrectas y reintentos <= 5| E
    C -->|ID bloqueado O superó los 5 reintentos| F

    %% Flujo de inactividad de sesión
    D -->|Inactividad por más de 20 min| G

    %% Resolución de la expiración de sesión
    G -->|No logra extender la sesión| H
    G -->|Extiende la sesión| Ibloqueo-de-sesion.png
```
