using System;

namespace ChimiMiBarriga
{
    // 1. CLASE BASE (Hamburguesa Clásica)
    public class Hamburguesa
    {
        // El constructor solo debe pedir estos tres según el enunciado
        protected string tipoPan;
        protected string carne;
        protected double precioBase;

        // Llevamos el conteo con variables individuales (hasta 4) 
        private string ad1Nombre; private double ad1Precio;
        private string ad2Nombre; private double ad2Precio;
        private string ad3Nombre; private double ad3Precio;
        private string ad4Nombre; private double ad4Precio;

        public Hamburguesa(string tipoPan, string carne, double precioBase)
        {
            this.tipoPan = tipoPan;
            this.carne = carne;
            this.precioBase = precioBase;
        }

        // Hacemos los métodos "virtual" para que la hamburguesa Premium 
        // pueda sobrescribirlos y bloquear que le metan más ingredientes.
        public virtual void AgregarAdicional1(string nombre, double precio) { ad1Nombre = nombre; ad1Precio = precio; }
        public virtual void AgregarAdicional2(string nombre, double precio) { ad2Nombre = nombre; ad2Precio = precio; }
        public virtual void AgregarAdicional3(string nombre, double precio) { ad3Nombre = nombre; ad3Precio = precio; }
        public virtual void AgregarAdicional4(string nombre, double precio) { ad4Nombre = nombre; ad4Precio = precio; }

        // Método para imprimir el ticket y devolver el total
        public virtual double MostrarTicket()
        {
            double total = precioBase;
            Console.WriteLine($"--- Ticket Chimi MiBarriga ---");
            Console.WriteLine($"Hamburguesa ({tipoPan}, de {carne}) - Base: ${precioBase}");

            // Si el ingrediente no está vacío, lo sumamos al total y lo mostramos
            if (ad1Nombre != null) { Console.WriteLine($" + {ad1Nombre}: ${ad1Precio}"); total += ad1Precio; }
            if (ad2Nombre != null) { Console.WriteLine($" + {ad2Nombre}: ${ad2Precio}"); total += ad2Precio; }
            if (ad3Nombre != null) { Console.WriteLine($" + {ad3Nombre}: ${ad3Precio}"); total += ad3Precio; }
            if (ad4Nombre != null) { Console.WriteLine($" + {ad4Nombre}: ${ad4Precio}"); total += ad4Precio; }

            return total;
        }
    }

    // 2. HAMBURGUESA SALUDABLE
    public class HamburguesaSaludable : Hamburguesa
    {
        // Procesar los 2 ingredientes extra en esta clase, NO en la base.
        private string ad5Nombre; private double ad5Precio;
        private string ad6Nombre; private double ad6Precio;

        // El constructor solo pide carne y precio, porque forzamos a que el pan sea Integral.
        public HamburguesaSaludable(string carne, double precioBase)
            : base("Pan Integral", carne, precioBase)
        {
        }

        public void AgregarAdicionalSaludable1(string nombre, double precio) { ad5Nombre = nombre; ad5Precio = precio; }
        public void AgregarAdicionalSaludable2(string nombre, double precio) { ad6Nombre = nombre; ad6Precio = precio; }

        public override double MostrarTicket()
        {
            // Primero calculamos lo de la clase padre (que ya maneja los primeros 4)
            double total = base.MostrarTicket();

            // Le sumamos los exclusivos de la saludable (llegando a 6 en total)
            if (ad5Nombre != null) { Console.WriteLine($" + {ad5Nombre} (Saludable): ${ad5Precio}"); total += ad5Precio; }
            if (ad6Nombre != null) { Console.WriteLine($" + {ad6Nombre} (Saludable): ${ad6Precio}"); total += ad6Precio; }

            return total;
        }
    }

    // 3. HAMBURGUESA PREMIUM
    public class HamburguesaPremium : Hamburguesa
    {
        public HamburguesaPremium(string tipoPan, string carne, double precioBase)
            : base(tipoPan, carne, precioBase)
        {
            // Agregar el combo automáticamente en la creación.
            // Usamos "base" para forzar la entrada antes de bloquear las funciones.
            base.AgregarAdicional1("Papitas", 50);
            base.AgregarAdicional2("Bebida", 40);
        }

        // Prevenir que se agreguen otros tras su creación.
        // Al hacer override a los métodos de agregar, anulamos su funcionamiento.
        public override void AgregarAdicional1(string nombre, double precio) { MostrarError(nombre); }
        public override void AgregarAdicional2(string nombre, double precio) { MostrarError(nombre); }
        public override void AgregarAdicional3(string nombre, double precio) { MostrarError(nombre); }
        public override void AgregarAdicional4(string nombre, double precio) { MostrarError(nombre); }

        private void MostrarError(string nombre)
        {
            Console.WriteLine($"[X] Denegado: No se puede agregar '{nombre}'. La Hamburguesa Premium no permite modificaciones.");
        }

        // No hace falta sobrescribir MostrarTicket() aquí porque 
        // la clase base imprimirá perfectamente el precio base + las papas y bebidas que forzamos.
    }

    // CLASE PRINCIPAL PARA PROBAR
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SISTEMA CHIMI MIBARRIGA - SR. BILLY NAVAJA ===");

            // --- PRUEBA 1: Chimi Clásico ---
            Hamburguesa chimiClasico = new Hamburguesa("Pan Normal", "Res", 150);
            chimiClasico.AgregarAdicional1("Doble Queso", 30);
            chimiClasico.AgregarAdicional2("Bacon", 40);
            chimiClasico.AgregarAdicional3("Pepinillos", 15);

            double total1 = chimiClasico.MostrarTicket();
            Console.WriteLine($"TOTAL A PAGAR: ${total1}");

            // --- PRUEBA 2: Chimi Saludable ---
            HamburguesaSaludable chimiSaludable = new HamburguesaSaludable("Pechuga de Pollo", 180);
            chimiSaludable.AgregarAdicional1("Tomate", 10);
            chimiSaludable.AgregarAdicional2("Lechuga", 10);
            // Usamos los métodos exclusivos para llegar a 6 en total
            chimiSaludable.AgregarAdicionalSaludable1("Aguacate Extra", 45);

            double total2 = chimiSaludable.MostrarTicket();
            Console.WriteLine($"TOTAL A PAGAR: ${total2}");

            // --- PRUEBA 3: Chimi Premium ---
            HamburguesaPremium chimiPremium = new HamburguesaPremium("Pan Brioche", "Angus", 300);

            // Intentamos engañar al sistema agregando un ingrediente
            Console.WriteLine("* Intentando agregar Bacon a la Premium... *");
            chimiPremium.AgregarAdicional3("Bacon", 40);

            double total3 = chimiPremium.MostrarTicket();
            Console.WriteLine($"TOTAL A PAGAR: ${total3}");

            Console.ReadLine();
        }
    }
}