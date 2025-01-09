using ChessWachinSSG.Data.Dtos;
using ChessWachinSSG.Model;

namespace ChessWachinSSG.HTML.Tags {

	/// <summary>
	/// Clase con métodos para los tag replacers de
	/// cuadros de playoffs.
	/// </summary>
	public static class Tr_PlayoffsDrawMethods {

		/// <summary>
		/// Añade los replacers al diccionario.
		/// </summary>
		/// <param name="dictionary">Diccionario original.</param>
		/// <param name="other">Diccionario con los elementos a añadir al original.</param>
		public static void AddReplacersRange(this Dictionary<string, ITagReplacer> dictionary, Dictionary<string, ITagReplacer> other) {
			foreach (var pair in other) {
				dictionary.Add(pair.Key, pair.Value);
			}
		}


		/// <param name="playoffs">Playoffs.</param>
		/// <returns>
		/// Replacers para los nombres de los jugadores
		/// y para sus puntuaciones totales.
		/// </returns>
		public static Dictionary<string, ITagReplacer> GetPlayerReplacers(Playoffs playoffs) {
			var output = new Dictionary<string, ITagReplacer>();

			output.AddReplacersRange(GetPlayerReplacers(playoffs, PlayoffRoundType.Semifinal1));
			output.AddReplacersRange(GetPlayerReplacers(playoffs, PlayoffRoundType.Semifinal2));
			output.AddReplacersRange(GetPlayerReplacers(playoffs, PlayoffRoundType.Final));

			return output;
		}

		/// <param name="playoffs">Playoffs.</param>
		/// <param name="type">Fase.</param>
		/// <returns>
		/// Replacers para los nombres de los jugadores
		/// y para sus puntuaciones totales para la
		/// fasee indicada.
		/// </returns>
		private static Dictionary<string, ITagReplacer> GetPlayerReplacers(Playoffs playoffs, PlayoffRoundType type) {
			var phase = type switch {
				PlayoffRoundType.Semifinal1 => playoffs.Semifinals1,
				PlayoffRoundType.Semifinal2 => playoffs.Semifinals2,
				PlayoffRoundType.Final => playoffs.Finals
			};

			if (phase == null || phase.Matches.GetAll().Count == 0) {
				return [];
			}

			var firstPlayer = phase.Matches.GetAll()[0].First;
			var secondPlayer = phase.Matches.GetAll()[0].Second;

			var firstPoints = phase.Ranking.GetPlayerInfo(firstPlayer.Id)?.Points.ToString() ?? string.Empty;
			var secondPoints = phase.Ranking.GetPlayerInfo(secondPlayer.Id)?.Points.ToString() ?? string.Empty;

			return new() {
				{ $"cwssg:playoffs:{GetPlayoffPhaseTypeStr(type)}:first", new Tr_Inline($"<{firstPlayer.NameTag}>") },
				{ $"cwssg:playoffs:{GetPlayoffPhaseTypeStr(type)}:second", new Tr_Inline($"<{secondPlayer.NameTag}>") },
				{ $"cwssg:playoffs:{GetPlayoffPhaseTypeStr(type)}:first:points", new Tr_Inline($"{firstPoints}") },
				{ $"cwssg:playoffs:{GetPlayoffPhaseTypeStr(type)}:second:points", new Tr_Inline($"{secondPoints}") }
			};
		}


		/// <param name="type">Fase.</param>
		/// <param name="match">Partida. Puede ser null si aún no se ha jugado.</param>
		/// <param name="index">Índice de la partida.</param>
		/// <returns>Replacers para el resultado de la partida.</returns>
		private static Dictionary<string, ITagReplacer> GetSingleResultReplacers(PlayoffRoundType type, Match? match, int index) {
			var firstResult = GetResultClass1(match).ToString();
			var secondResult = GetResultClass2(match).ToString();

			return new() {
				{ $"cwssg:playoffs:{GetPlayoffPhaseTypeStr(type)}:result:first:{index}", new Tr_Inline(index % 2 == 0 ? firstResult : secondResult) },
				{ $"cwssg:playoffs:{GetPlayoffPhaseTypeStr(type)}:result:second:{index}", new Tr_Inline(index % 2 == 1 ? firstResult : secondResult) }
			};
		}


		/// <param name="playoffs">Playoffs.</param>
		/// <returns>Replacers para los resultados de todas las partidas.</returns>
		public static Dictionary<string, ITagReplacer> GetResultReplacers(Playoffs playoffs) {
			var output = new Dictionary<string, ITagReplacer>();

			output.AddReplacersRange(GetResultReplacers(playoffs, PlayoffRoundType.Semifinal1));
			output.AddReplacersRange(GetResultReplacers(playoffs, PlayoffRoundType.Semifinal2));
			output.AddReplacersRange(GetResultReplacers(playoffs, PlayoffRoundType.Final));

			return output;
		}

		/// <param name="playoffs">Playoffs.</param>
		/// <param name="type">Fase.</param>
		/// <returns>Replacers para todos los resultados de las partidas de la fase indicada.</returns>
		private static Dictionary<string, ITagReplacer> GetResultReplacers(Playoffs playoffs, PlayoffRoundType type) {
			var output = new Dictionary<string, ITagReplacer>();

			var phase = type switch {
				PlayoffRoundType.Semifinal1 => playoffs.Semifinals1,
				PlayoffRoundType.Semifinal2 => playoffs.Semifinals2,
				PlayoffRoundType.Final => playoffs.Finals
			};

			for (int i = 0; i < playoffs.DefaultDurations.Count; i++) {
				output.AddReplacersRange(GetSingleResultReplacers(type, phase?.Matches.GetAt(i), i));
			}

			return output;
		}


		/// <param name="playoffs">Playoffs.</param>
		/// <param name="type">Fase.</param>
		/// <returns>Replacers para las duraciones de las partidas de la fase indicada.</returns>
		private static Dictionary<string, ITagReplacer> GetDurationReplacers(Playoffs playoffs, PlayoffRoundType type) {
			var output = new Dictionary<string, ITagReplacer>();

			var phase = type switch {
				PlayoffRoundType.Semifinal1 => playoffs.Semifinals1,
				PlayoffRoundType.Semifinal2 => playoffs.Semifinals2,
				PlayoffRoundType.Final => playoffs.Finals
			};

			for (int i = 0; i < playoffs.DefaultDurations.Count; i++) {
				output.AddReplacersRange(GetSingleDurationReplacer(playoffs, phase, type, i));
			}

			return output;
		}

		/// <param name="playoffs">Playoffs.</param>
		/// <returns>Replacers para las duraciones de todas las partidas.</returns>
		public static Dictionary<string, ITagReplacer> GetDurationReplacers(Playoffs playoffs) {
			var output = new Dictionary<string, ITagReplacer>();

			output.AddReplacersRange(GetDurationReplacers(playoffs, PlayoffRoundType.Semifinal1));
			output.AddReplacersRange(GetDurationReplacers(playoffs, PlayoffRoundType.Semifinal2));
			output.AddReplacersRange(GetDurationReplacers(playoffs, PlayoffRoundType.Final));

			return output;
		}

		/// <param name="playoffs">Playoffs.</param>
		/// <param name="phase">Fase.</param>
		/// <param name="type">Fase.</param>
		/// <param name="index">Índice de la partida dentro de la fase.</param>
		/// <returns>Replacer para la duración de una sola partida.</returns>
		private static Dictionary<string, ITagReplacer> GetSingleDurationReplacer(Playoffs playoffs, PlayoffsRound? phase, PlayoffRoundType type, int index) {
			return new() {
				{ $"cwssg:playoffs:{GetPlayoffPhaseTypeStr(type)}:durations:{index}", new Tr_Inline(GetDurationOrDefault(playoffs, phase, index)) }
			};
		}

		/// <param name="playoffs">Playoffs.</param>
		/// <returns>Replacers para las clases de los resultados de cada fase.</returns>
		public static Dictionary<string, ITagReplacer> GetOverallResultReplacers(Playoffs playoffs) {
			var output = new Dictionary<string, ITagReplacer>();

			output.AddReplacersRange(GetOverallResultReplacers(playoffs, PlayoffRoundType.Semifinal1));
			output.AddReplacersRange(GetOverallResultReplacers(playoffs, PlayoffRoundType.Semifinal2));
			output.AddReplacersRange(GetOverallResultReplacers(playoffs, PlayoffRoundType.Final));

			return output;
		}

		/// <param name="playoffs">Playoffs.</param>
		/// <param name="type">Fase.</param>
		/// <returns>Replacers para las clases de los resultados de la fase indicada.</returns>
		private static Dictionary<string, ITagReplacer> GetOverallResultReplacers(Playoffs playoffs, PlayoffRoundType type) {
			var phase = type switch {
				PlayoffRoundType.Semifinal1 => playoffs.Semifinals1,
				PlayoffRoundType.Semifinal2 => playoffs.Semifinals2,
				PlayoffRoundType.Final => playoffs.Finals
			};

			if (phase == null || phase.Matches.GetAll().Count == 0) {
				return [];
			}

			var firstPlayer = phase.Matches.GetAll()[0].First;
			var secondPlayer = phase.Matches.GetAll()[0].Second;

			return new() {
				{ $"cwssg:playoffs:{GetPlayoffPhaseTypeStr(type)}:result:first", new Tr_Inline(GetPhaseResultClass(phase.Ranking.GetPlayerPosition(firstPlayer))) },
				{ $"cwssg:playoffs:{GetPlayoffPhaseTypeStr(type)}:result:second", new Tr_Inline(GetPhaseResultClass(phase.Ranking.GetPlayerPosition(secondPlayer))) },
			};
		}


		/// <param name="type">Fase.</param>
		/// <returns>ID interno de la fase dentro de los tags.</returns>
		private static string GetPlayoffPhaseTypeStr(PlayoffRoundType type) => type switch {
			PlayoffRoundType.Semifinal1 => "sf1",
			PlayoffRoundType.Semifinal2 => "sf2",
			PlayoffRoundType.Final => "finals"
		};


		/// <param name="result">Resultado de una partida.</param>
		/// <returns>Clase CSS que debe tener el primer jugador de la partida.</returns>
		private static string GetResultClass1(Match? result) {
			if (result == null) {
				return NoMatchClass;
			}

			return result.Result switch {
				Winner.First => "round-win",
				Winner.Draw => "round-draw",
				Winner.Second => "round-loss",
			};
		}

		/// <param name="result">Resultado de una partida.</param>
		/// <returns>Clase CSS que debe tener el segundo jugador de la partida.</returns>
		private static string GetResultClass2(Match? result) {
			if (result == null) {
				return NoMatchClass;
			}

			return result.Result switch {
				Winner.First => "round-loss",
				Winner.Draw => "round-draw",
				Winner.Second => "round-win",
			};
		}

		/// <param name="position">Posición del jugador en el ranking de una fase.</param>
		/// <returns>Clase que tiene según si pasa a la siguiente ronda o no.</returns>
		private static string GetPhaseResultClass(int position) {
			return position switch {
				0 => BracketWinner,
				1 => BracketLoser,
				_ => ""
			};
		}

		/// <param name="playoffs">Playoffs.</param>
		/// <param name="phase">Ronda.</param>
		/// <param name="matchIdx">Índice de la partida.</param>
		/// <returns>
		/// Duración de la partida.
		/// Si no se ha jugado, devuelve la duración por defecto.
		/// </returns>
		private static string GetDurationOrDefault(Playoffs playoffs, PlayoffsRound? phase, int matchIdx) {
			if (phase == null) {
				return playoffs.DefaultDurations[matchIdx];
			}

			var match = phase.Matches.GetAt(matchIdx);

			if (match == null) {
				return phase.Parent!.DefaultDurations[matchIdx];
			}

			return (match.DurationTypeSeconds / 60).ToString();
		}

		private const string NoMatchClass = "round-none";
		private const string BracketWinner = "bracket-winner";
		private const string BracketLoser = "bracket-loser";

	}
}
