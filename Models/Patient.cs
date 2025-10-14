using Gestion_Hospital.Interfaces;

namespace Gestion_Hospital.Models
{
	/// <summary>
	/// Representa un paciente en el sistema.
	/// Hereda de <see cref="Person"/> para datos comunes (nombre, documento, teléfono, email)
	/// e implementa <see cref="Gestion_Hospital.Interfaces.IRegistrable"/> para marcarlo como entidad registrable/persistible.
	/// </summary>
	public class Patient : Person, IRegistrable
	{
		/// <summary>Edad del paciente (años).</summary>
		public int Age { get; set; }
	}
}
