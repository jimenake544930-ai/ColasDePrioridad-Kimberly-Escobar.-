using System;
using System.Text;

namespace ColasDePrioridad_Kimberly_Escobar
{

    class Program
    {
               static MinHeap cola = new MinHeap();

        static int contadorLlegada = 0;

        // MAIN: aquí empieza a ejecutarse el programa.
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // Mientras sea true, el menú se vuelve a mostrar después de cada acción.
            bool ejecutando = true;

            while (ejecutando)
            {
                MostrarMenu();

                // Leemos lo que el usuario escribe.
                string opcion = Console.ReadLine();

                // Según la opción escogida, se llama a la función correspondiente.
                switch (opcion)
                {
                    case "1":
                        RegistrarTicket();
                        break;
                    case "2":
                        MostrarSiguienteTicket();
                        break;
                    case "3":
                        AtenderTicket();
                        break;
                    case "4":
                        MostrarColaDePrioridad();
                        break;
                    case "5":
                        BuscarTicket();
                        break;
                    case "6":
                        MostrarCantidadDeTickets();
                        break;
                    case "7":
                        Console.WriteLine();
                        Mensaje("Saliendo del sistema. ¡Hasta pronto!", ConsoleColor.Green);
                        ejecutando = false; // esto termina el programa
                        continue;
                    default:
                        Console.WriteLine();
                        Mensaje("Opción inválida. Debe elegir un número del 1 al 7.", ConsoleColor.Red); 
                        break;
                }

                // Pausa para que el usuario pueda leer el resultado.
                Pausar();
            }
        }

        
        static void Mensaje(string texto, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(texto);
            Console.ResetColor(); //
        }

        static void Pregunta(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(texto);
            Console.ResetColor();
        }

        static void Encabezado(string titulo)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("========================================");
            Console.WriteLine(titulo);
            Console.WriteLine("========================================");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void Separador()
        {
            Mensaje("----------------------------------------", ConsoleColor.DarkGray);
        }

        // Escribe una opción del menú: el número con color y el texto normal.
        static void OpcionMenu(string numero, string texto, ConsoleColor colorNumero)
        {
            Console.ForegroundColor = colorNumero;
            Console.Write(numero + " ");
            Console.ResetColor();
            Console.WriteLine(texto);
        }

        // ====================================================================
        // MENÚ PRINCIPAL
        // ====================================================================
        static void MostrarMenu()
        {
            Console.Clear(); // Limpia la pantalla

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==================================================");
            Console.WriteLine("        SISTEMA DE GESTIÓN DE TICKETS DE SOPORTE");
            Console.WriteLine("                 TechSolucion");
            Console.WriteLine("==================================================");
            Console.ResetColor();
            Console.WriteLine();

            OpcionMenu("[1]", "Registrar Ticket", ConsoleColor.Green);
            OpcionMenu("[2]", "Mostrar Siguiente Ticket", ConsoleColor.Cyan);
            OpcionMenu("[3]", "Atender Ticket", ConsoleColor.Magenta);
            OpcionMenu("[4]", "Mostrar Cola de Prioridad", ConsoleColor.Yellow);
            OpcionMenu("[5]", "Buscar Ticket", ConsoleColor.DarkYellow);
            OpcionMenu("[6]", "Mostrar Cantidad de Tickets", ConsoleColor.DarkCyan);
            OpcionMenu("[7]", "Salir", ConsoleColor.Red);
            Console.WriteLine();

            Mensaje("--------------------------------------------------", ConsoleColor.DarkGray);
            Pregunta("Seleccione una opción: ");
        }

        // ====================================================================
        // OPCIÓN 1: REGISTRAR TICKET
        // ====================================================================
        static void RegistrarTicket()
        {
            Encabezado("            REGISTRO DE TICKET");

            string codigo = "";
            bool codigoValido = false;

            // Repetimos la pregunta hasta que el código sea válido.
            while (!codigoValido)
            {
                Pregunta("Código del Ticket (ej. TCK0005): ");

                codigo = (Console.ReadLine() ?? "").Trim().ToUpper();

                if (!TieneFormatoValido(codigo))
                {
                    Mensaje("  -> Formato inválido. Debe ser TCK seguido de 4 dígitos (ej. TCK0001).", ConsoleColor.Red);
                }
                else if (cola.Buscar(codigo) != null)
                {
                    Mensaje("  -> Ese código ya existe. No se permiten códigos duplicados.", ConsoleColor.Red);
                }
                else
                {
                    codigoValido = true; 
                }
            }

            // ---------- 2) CLIENTE ----------
            string cliente = LeerTextoNoVacio("Nombre del Cliente: ");

            // ---------- 3) DESCRIPCIÓN ----------
            string descripcion = LeerTextoNoVacio("Descripción del Problema: ");

            // ---------- 4) PRIORIDAD ----------
            int prioridad = 0;
            bool prioridadValida = false;

            while (!prioridadValida)
            {
                Pregunta("Nivel de Prioridad (1-5): ");
                string texto = Console.ReadLine();

                if (int.TryParse(texto, out prioridad) && prioridad >= 1 && prioridad <= 5)
                {
                    prioridadValida = true;
                }
                else
                {
                    Mensaje("  -> Prioridad inválida. Debe ser un número entre 1 y 5.", ConsoleColor.Red);
                }
            }

            // ---------- 5) CREAR E INSERTAR ----------
            contadorLlegada++; // este ticket es el número de llegada más nuevo

            // Creamos el ticket usando el molde de la clase Ticket.
            Ticket nuevo = new Ticket(codigo, cliente, descripcion, prioridad, contadorLlegada);

            // Lo insertamos en el Min Heap (adentro hace el Bubble Up).
            cola.Insertar(nuevo);

            // ---------- 6) MOSTRAR RESULTADO ----------
            Encabezado("            REGISTRO DE TICKET");
            nuevo.MostrarDetalle();
            Console.WriteLine();
            Separador();
            Console.WriteLine();
            Mensaje("Ticket registrado exitosamente.", ConsoleColor.Green);
            Console.WriteLine();
            Separador();
        }

        // ====================================================================
        // OPCIÓN 2: MOSTRAR SIGUIENTE TICKET 
        // ====================================================================
        static void MostrarSiguienteTicket()
        {
            Encabezado("         SIGUIENTE TICKET A ATENDER");

            Ticket siguiente = cola.VerMinimo();

            if (siguiente == null)
            {
                Mensaje("La cola de prioridad está vacía. No hay tickets pendientes.", ConsoleColor.Yellow);
            }
            else
            {
                Mensaje("RESULTADO DE LA CONSULTA", ConsoleColor.Magenta);
                Separador();
                Console.WriteLine();
                siguiente.MostrarDetalle();
                Console.WriteLine();
                Separador();
            }
        }

        // ====================================================================
        // OPCIÓN 3: ATENDER TICKET (ExtractMin)
        // ====================================================================
        static void AtenderTicket()
        {
            Encabezado("             ATENDER TICKET");

            Ticket atendido = cola.ExtraerMinimo();

            if (atendido == null)
            {
                Mensaje("La cola de prioridad está vacía. No hay tickets para atender.", ConsoleColor.Yellow);
            }
            else
            {
                Mensaje("RESULTADO DE LA OPERACIÓN", ConsoleColor.Magenta);
                Separador();
                Console.WriteLine();
                atendido.MostrarDetalle();
                Console.WriteLine();
                Separador();
                Console.WriteLine();
                Mensaje("Ticket atendido correctamente.", ConsoleColor.Green);
                Console.WriteLine();
                Separador();
            }
        }

        // ====================================================================
        // OPCIÓN 4: MOSTRAR COLA DE PRIORIDAD
        // ====================================================================
        static void MostrarColaDePrioridad()
        {
            Encabezado("          COLA DE PRIORIDAD ACTUAL");

            if (cola.EstaVacio())
            {
                Mensaje("No existen tickets registrados.", ConsoleColor.Yellow);
                return; 
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("{0,-10}{1,-12}{2,-25}{3}", "Posición", "Ticket", "Cliente", "Prioridad");
            Console.ResetColor();
            Mensaje("------------------------------------------------------------", ConsoleColor.DarkGray);

            for (int i = 0; i < cola.Cantidad; i++)
            {
                Ticket t = cola.ObtenerEnPosicion(i);

                Console.Write("{0,-10}{1,-12}{2,-25}", i, t.Codigo, t.Cliente);

                // prioridad con el color que le corresponde.
                Console.ForegroundColor = t.ObtenerColorPrioridad();
                Console.WriteLine(t.Prioridad);
                Console.ResetColor();
            }

            Mensaje("------------------------------------------------------------", ConsoleColor.DarkGray);
            Mensaje("Total de Tickets: " + cola.Cantidad, ConsoleColor.Green);
        }

        // ====================================================================
        // OPCIÓN 5: BUSCAR TICKET
        // ====================================================================
        static void BuscarTicket()
        {
            Encabezado("           BÚSQUEDA DE TICKET");

            Pregunta("Ingrese el código del ticket: ");
            string codigo = (Console.ReadLine() ?? "").Trim().ToUpper();
            Console.WriteLine();

            // Buscar recorre el heap sin modificarlo.
            Ticket encontrado = cola.Buscar(codigo);

            if (encontrado == null)
            {
                Mensaje("No se encontró ningún ticket con el código " + codigo + ".", ConsoleColor.Red);
            }
            else
            {
                Mensaje("RESULTADO DE LA BÚSQUEDA", ConsoleColor.Magenta);
                Separador();
                Console.WriteLine();
                encontrado.MostrarDetalle();
                Console.WriteLine();
                Separador();
            }
        }

        // ====================================================================
        // OPCIÓN 6: MOSTRAR CANTIDAD DE TICKETS
        // ====================================================================
        static void MostrarCantidadDeTickets()
        {
            Encabezado("           CANTIDAD DE TICKETS");

            Mensaje("TOTAL DE TICKETS REGISTRADOS", ConsoleColor.Magenta);
            Separador();
            Console.WriteLine();

            // Si no hay tickets, Cantidad vale 0 y se muestra 0 automáticamente.
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Cantidad de Tickets : ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(cola.Cantidad);
            Console.ResetColor();

            Console.WriteLine();
            Separador();
        }        static void Pausar()
        {
            Console.WriteLine();
            Mensaje("Presione Enter para volver al menú...", ConsoleColor.DarkGray);
            Console.ReadLine();
        }

        // Pide un texto y repite la pregunta si el usuario lo deja vacío.
        static string LeerTextoNoVacio(string mensaje)
        {
            string texto = "";

            while (string.IsNullOrWhiteSpace(texto))
            {
                Pregunta(mensaje);
                texto = (Console.ReadLine() ?? "").Trim();

                if (string.IsNullOrWhiteSpace(texto))
                {
                    Mensaje("  -> Este campo no puede estar vacío.", ConsoleColor.Red);
                }
            }

            return texto;
        }

        // Verifica que un código tenga el formato TCK + 4 dígitos.
        // Ejemplo válido: TCK0005. Ejemplos inválidos: TCK5, ABC0005, TCK00A5.
        static bool TieneFormatoValido(string codigo)
        {
            if (codigo.Length != 7 || !codigo.StartsWith("TCK"))
            {
                return false;
            }

            for (int i = 3; i < 7; i++)
            {
                if (!char.IsDigit(codigo[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

namespace ColasDePrioridad_Kimberly_Escobar
{
    // ========================================================================
    // CLASE TICKET: 
    // ========================================================================
    public class Ticket
    {

        public string Codigo { get; private set; }
        public string Cliente { get; private set; }
        public string Descripcion { get; private set; }
        public int Prioridad { get; private set; }

        public int NumeroLlegada { get; private set; }

        public Ticket(string codigo, string cliente, string descripcion, int prioridad, int numeroLlegada)
        {
            this.Codigo = codigo;
            this.Cliente = cliente;
            this.Descripcion = descripcion;
            this.Prioridad = prioridad;
            this.NumeroLlegada = numeroLlegada;
        }

        // Convierte el número de prioridad en su nombre 1 = Crítica, etc..
        public string ObtenerNombrePrioridad()
        {
            switch (Prioridad)
            {
                case 1: return "Crítica";
                case 2: return "Alta";
                case 3: return "Media";
                case 4: return "Baja";
                case 5: return "Muy Baja";
                default: return "Desconocida";
            }
        }

        // Devuelve el color que corresponde a cada prioridad:
        // 1 = rojo (urgente) o  5 = turquesa (tranquilo).
        public ConsoleColor ObtenerColorPrioridad()
        {
            switch (Prioridad)
            {
                case 1: return ConsoleColor.Red;
                case 2: return ConsoleColor.DarkYellow; // naranja
                case 3: return ConsoleColor.Yellow;
                case 4: return ConsoleColor.Green;
                case 5: return ConsoleColor.Cyan;
                default: return ConsoleColor.White;
            }
        }
        public bool TieneMasUrgenciaQue(Ticket otro)
        {
            if (this.Prioridad != otro.Prioridad)
            {
                return this.Prioridad < otro.Prioridad;
            }

            return this.NumeroLlegada < otro.NumeroLlegada;
        }
        private void EscribirCampo(string etiqueta, string valor, ConsoleColor colorValor)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(etiqueta.PadRight(18) + ": "); // 
            Console.ForegroundColor = colorValor;
            Console.WriteLine(valor);
            Console.ResetColor();
        }

        // Imprime los datos del ticket en pantalla, con colores.
        public void MostrarDetalle()
        {
            EscribirCampo("Código del Ticket", Codigo, ConsoleColor.White);
            EscribirCampo("Cliente", Cliente, ConsoleColor.White);
            EscribirCampo("Descripción", Descripcion, ConsoleColor.White);
            EscribirCampo("Prioridad", Prioridad + " (" + ObtenerNombrePrioridad() + ")", ObtenerColorPrioridad());
        }
    }
}

namespace ColasDePrioridad_Kimberly_Escobar
{
    // ========================================================================
    // CLASE MINHEAP: implementación propia de un Min Heap
    // ------------------------------------------------------------------------
    // Es un árbol donde el ticket más urgente siempre está arriba (raíz).
    // Se guarda en un arreglo. Fórmulas de posiciones:
    //     Hijo izquierdo de i -> 2 * i + 1
    //     Hijo derecho de i   -> 2 * i + 2
    //     Padre de i          -> (i - 1) / 2
    // ========================================================================
    public class MinHeap
    {
        // Arreglo donde se guardan los tickets.
        private Ticket[] elementos;

        // Cuántos tickets hay guardados actualmente.
        private int cantidad;

        // Permite a otras clases leer la cantidad (sin poder modificarla).
        public int Cantidad
        {
            get { return cantidad; }
        }
        public MinHeap()
        {
            elementos = new Ticket[10];
            cantidad = 0;
        }

        // Devuelve true si no hay ningún ticket guardado.
        public bool EstaVacio()
        {
            return cantidad == 0;
        }

        // ==================== INSERTAR (+ Bubble Up) ====================
        // 1) Si el arreglo está lleno, lo agranda.
        // 2) Coloca el ticket en la siguiente posición libre (al final).
        // 3) Ejecuta BubbleUp para subirlo si es más urgente que su padre.
        public void Insertar(Ticket nuevo)
        {
            if (cantidad == elementos.Length)
            {
                AgrandarArreglo();
            }

            elementos[cantidad] = nuevo;
            cantidad++;

            BubbleUp(cantidad - 1);
        }

        // BUBBLE UP: sube el elemento mientras sea más urgente que su padre.
        private void BubbleUp(int indice)
        {
            while (indice > 0)
            {
                int padre = (indice - 1) / 2;

                if (elementos[indice].TieneMasUrgenciaQue(elementos[padre]))
                {
                    Intercambiar(indice, padre);
                    indice = padre; 
                }
                else
                {
                    break; 
                }
            }
        }
        public Ticket VerMinimo()
        {
            if (EstaVacio())
            {
                return null;
            }

            return elementos[0];
        }

        // ==================== EXTRAER MÍNIMO (+ Bubble Down) ====================
        // 1) Guarda la raíz para devolverla.
        // 2) Mueve el último ticket a la raíz.
        // 3) Reduce la cantidad en 1.
        // 4) Ejecuta BubbleDown para restaurar la regla del Min Heap.
        public Ticket ExtraerMinimo()
        {
            if (EstaVacio())
            {
                return null;
            }

            Ticket raiz = elementos[0];

            elementos[0] = elementos[cantidad - 1];
            elementos[cantidad - 1] = null;
            cantidad--;

            if (cantidad > 0)
            {
                BubbleDown(0);
            }

            return raiz;
        }

        // BUBBLE DOWN: baja el elemento intercambiándolo con su hijo más
        // urgente, mientras algún hijo sea más urgente que él.
        private void BubbleDown(int indice)
        {
            while (true)
            {
                int hijoIzquierdo = 2 * indice + 1;
                int hijoDerecho = 2 * indice + 2;

                int masUrgente = indice;

                if (hijoIzquierdo < cantidad &&
                    elementos[hijoIzquierdo].TieneMasUrgenciaQue(elementos[masUrgente]))
                {
                    masUrgente = hijoIzquierdo;
                }

                if (hijoDerecho < cantidad &&
                    elementos[hijoDerecho].TieneMasUrgenciaQue(elementos[masUrgente]))
                {
                    masUrgente = hijoDerecho;
                }

                if (masUrgente == indice)
                {
                    break;
                }

                Intercambiar(indice, masUrgente);
                indice = masUrgente;
            }
        }

        public Ticket ObtenerEnPosicion(int posicion)
        {
            if (posicion < 0 || posicion >= cantidad)
            {
                return null;
            }

            return elementos[posicion];
        }

        // ==================== BUSCAR por código ====================
        // Devuelve el ticket si existe, o null si no.
        public Ticket Buscar(string codigo)
        {
            for (int i = 0; i < cantidad; i++)
            {
                if (string.Equals(elementos[i].Codigo, codigo, StringComparison.OrdinalIgnoreCase))
                {
                    return elementos[i];
                }
            }

            return null;
        }

        // Intercambia dos tickets de posición usando una variable temporal.
        private void Intercambiar(int a, int b)
        {
            Ticket temporal = elementos[a];
            elementos[a] = elementos[b];
            elementos[b] = temporal;
        }

        // Cuando el arreglo se llena, crea uno del doble de tamaño y copia
        // ahí todos los tickets.
        private void AgrandarArreglo()
        {
            Ticket[] nuevoArreglo = new Ticket[elementos.Length * 2];

            for (int i = 0; i < cantidad; i++)
            {
                nuevoArreglo[i] = elementos[i];
            }

            elementos = nuevoArreglo;
        }
    }
}