using ChessWachinSSG.Data;
using ChessWachinSSG.HTML;
using ChessWachinSSG.HTML.Tags;
using ChessWachinSSG.Model;

using System.Text.Json;
using ChessWachinSSG.Data.Dtos;
using ChessWachinSSG.Data.Daos;
using System.Reflection;

namespace ChessWachinSSG {

	/// <summary>
	/// Clase inicial del ChessWachinSSG.
	/// </summary>
	internal static class Main {

		public static void Run(string[] args) {
			logger.Info($"Bienvenido a ChessWachinSSG {Assembly.GetExecutingAssembly().GetName().Version}");

			if (!File.Exists("config.json")) {
				logger.Error("No se puede leer el archivo de configuración config.json. Finalizando proceso.");
				return;
			}

			string inputPath;
			string outputPath;

			using (StreamReader r = new("./config.json")) {
				JsonDocument json = JsonDocument.Parse(r.ReadToEnd());

				try {
					inputPath = json.RootElement.GetProperty("input").GetString()!;
					outputPath = json.RootElement.GetProperty("output").GetString()!;
				}
				catch (Exception e) {
					logger.Error(e, "Error al leer config.json. Finalizando proceso.");
					return;
				}
			}

			// Construcción del contexto.
			var contextBuilder = new Context.Builder();
			contextBuilder.ApplyDaos(
				CountryDao.FromFile("Data/countries.json", new FileReader()),
				PlayerDao.FromFile("Data/players.json", new FileReader()),
				TagDao.FromFile("Data/tags.json", new FileReader()),
				CompetitionDao.FromFile("Data/competitions.json", new FileReader()));

			// Actualización de ELO (opcional).
			if (args.Contains("-eloupdate")) {
				List<Task<EloDto>> elos = [];
				List<EloDto> elosDatas = [];

				foreach (var (_, player) in contextBuilder.GetCurrentPlayers()) {
					var elo = PlayerEloRequester.GetPlayerElos(player.Name, player.Id);
					elos.Add(elo);
				}

				foreach (var elo in elos) {
					elosDatas.Add(elo.Result);
				}

				var dao = new EloDao((elosDatas, $"{DateTime.Now:yyyy-MM-dd h:mm:ss}"));
				dao.Save("Data/elo_cache.json");

				contextBuilder.SetElos(dao.GetAllElos(), dao.GetDate());
			}
			else {
				var data = EloDao.FromFile("Data/elo_cache.json", new FileReader());
				contextBuilder.SetElos(data.GetAllElos(), data.GetDate());
			}

			var context = contextBuilder.Build();


			// Procesado de la página de origen.
			ProcessDirectory(inputPath, inputPath, outputPath, context);


			// Creación de páginas de perfiles.
			logger.Debug("Generando perfiles.");
			foreach (var (id, player) in context.Players) {
				var html = $"<cwssg:profile {id}>";
				Dictionary<string, ITagReplacer> replacers = new() {
					{ "cwssg:profile", new Tr_Profile(new FileReader()) }
				};

				WriteFile($"WebOut/perfiles/{player.Name}.html", HtmlBuilder.Process(html, context, replacers));
			}

			logger.Debug("Proceso finalizado.");
		}

		/// <summary>
		/// Procesa todos los archivos de un directorio
		/// de manera recursiva.
		/// </summary>
		/// <param name="directory">Directorio original.</param>
		/// <param name="inputPath">Directorio ROOT procesado.</param>
		/// <param name="outputPath">Ruta de destino.</param>
		/// <param name="context">Contexto del programa.</param>
		private static void ProcessDirectory(string directory, string inputPath, string outputPath, Context context) {
			logger.Trace($"Procesando el directorio {directory}.");

			var files = Directory.GetFiles(directory);

			foreach (var file in files) {

				// Ruta relativa respecto al directorio ROOT.
				string relativePath = file[inputPath.Length..];

				if (file.EndsWith(".html")) {
					ProcessHtml(file, outputPath + relativePath, context);
				}
				else {
					if (!Directory.Exists(outputPath + relativePath)) {
						Directory.CreateDirectory(Path.GetDirectoryName(outputPath + relativePath)!);
					}

					File.Copy(file, outputPath + relativePath, true);
				}
			}

			// Recursivo.
			var directories = Directory.GetDirectories(directory);
			foreach (var d in directories) {
				ProcessDirectory(d, inputPath, outputPath, context);
			}
		}

		/// <summary>
		/// Procesa los tags de un archivo HTML,
		/// y después guarda el resultado.
		/// </summary>
		/// <param name="path">Ruta del archivo HTML original.</param>
		/// <param name="outputPath">Ruta en la que se guardará el archivo modificado.</param>
		/// <param name="context">Contexto del programa.</param>
		private static void ProcessHtml(string path, string outputPath, Context context) {
			logger.Trace($"Procesando archivo {path} para escribirlo en {outputPath}.");
			var content = File.ReadAllText(path);
			var output = HtmlBuilder.Process(content, context);

			WriteFile(outputPath, output);
		}

		/// <summary>
		/// Escribe los contenidos de un texto
		/// en un archivo externo.
		/// 
		/// Se sobreescribe si ya existe.
		/// </summary>
		/// <param name="path">Ruta del archivo a escribir.</param>
		/// <param name="text">Texto a escribir.</param>
		private static void WriteFile(string path, string text) {
			if (!Directory.Exists(path)) {
				Directory.CreateDirectory(Path.GetDirectoryName(path)!);
			}

			File.WriteAllText(path, text);
		}

		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	}
}
