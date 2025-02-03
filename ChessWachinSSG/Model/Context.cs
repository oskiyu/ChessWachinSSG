using ChessWachinSSG.Data;
using ChessWachinSSG.Data.Daos;
using ChessWachinSSG.Data.Dtos;
using ChessWachinSSG.HTML.Tags;

using System.Reflection;

namespace ChessWachinSSG.Model {

	/// <summary>
	/// Clase que contiene todos los datos
	/// del Chess-Wachín.
	/// </summary>
	public class Context {

		/// <summary>
		/// Instancia vacía.
		/// </summary>
		public static Context Empty { get => new(); }


		/// <summary>
		/// Clase auxilair para la construcción del contexto.
		/// </summary>
		public class Builder {

			/// <summary>
			/// Construye el contexto a partir de los daos indicados.
			/// </summary>
			public Builder ApplyDaos(CountryDao countriesDao, PlayerDao playersDao, TagDao tagDao, CompetitionDao competitionDao, NewsDao newsDao) {
				logger.Trace($"Cargando contexto.");

				// Carga de países.
				var countries = countriesDao.GetAllCountries();
				foreach (var c in countries) {
					instance._countries[c.Id] = new(c.Id, c.Name, c.FlagIconPath, c.PlayerCardClass);
				}

				// Carga de jugadores.
				var players = playersDao.GetAllPlayers();
				foreach (var p in players) {
					instance._players[p.Id] = new(p.Id, p.Name, instance.Countries[p.CountryId], p.PfpPath, p.NameTag);
				}

				// Carga de competiciones.
				var competitions = competitionDao.GetAllCompetitions();
				foreach (var c in competitions) {

					// Carga de las partidas de la fase de liga.
					var leagueMatchesBuilder = new MatchList.Builder();
					var desempateMatchesBuilder = new MatchList.Builder();
					if (c.League != null) {
						foreach (var m in MatchDao.FromFile(c.League!.MatchesPath, new FileReader()).GetAllMatches()) {
							leagueMatchesBuilder.AddMatch(
								new(
									instance.Players[m.FirstPlayerId],
									instance.Players[m.SecondPlayerId],
									m.Winner,
									m.Moves,
									m.Duration,
									m.DurationType,
									m.Url,
									m.Date));
						}

						if (c.League!.DesempateMatchesPath != null) {
							foreach (var m in MatchDao.FromFile(c.League!.DesempateMatchesPath, new FileReader()).GetAllMatches()) {
								desempateMatchesBuilder.AddMatch(
									new(
										instance.Players[m.FirstPlayerId],
										instance.Players[m.SecondPlayerId],
										m.Winner,
										m.Moves,
										m.Duration,
										m.DurationType,
										m.Url,
										m.Date));
							}
						}
					}

					// Carga de las partidas de la primera semifinal.
					var sf1MatchesBuilder = new MatchList.Builder();
					if (c.Playoffs?.Semifinals1 != null) {
						foreach (var m in MatchDao.FromFile(c.Playoffs.Semifinals1.MatchesPath, new FileReader()).GetAllMatches()) {
							sf1MatchesBuilder.AddMatch(
								new(
									instance.Players[m.FirstPlayerId],
									instance.Players[m.SecondPlayerId],
									m.Winner,
									m.Moves,
									m.Duration,
									m.DurationType,
									m.Url,
									m.Date));
						}
					}

					// Carga de las partidas de la segunda semifinal.
					var sf2MatchesBuilder = new MatchList.Builder();
					if (c.Playoffs?.Semifinals2 != null) {
						foreach (var m in MatchDao.FromFile(c.Playoffs.Semifinals2.MatchesPath, new FileReader()).GetAllMatches()) {
							sf2MatchesBuilder.AddMatch(
								new(
									instance.Players[m.FirstPlayerId],
									instance.Players[m.SecondPlayerId],
									m.Winner,
									m.Moves,
									m.Duration,
									m.DurationType,
									m.Url,
									m.Date));
						}
					}

					// Carga de las partidas de la final.
					var finalsMatchesBuilder = new MatchList.Builder();
					if (c.Playoffs?.Finals != null) {
						foreach (var m in MatchDao.FromFile(c.Playoffs.Finals.MatchesPath, new FileReader()).GetAllMatches()) {
							finalsMatchesBuilder.AddMatch(
								new(
									instance.Players[m.FirstPlayerId],
									instance.Players[m.SecondPlayerId],
									m.Winner,
									m.Moves,
									m.Duration,
									m.DurationType,
									m.Url,
									m.Date));
						}
					}

					var leagueMatches = leagueMatchesBuilder.Build();
					League? league = leagueMatches.GetAll().Count switch {
						0 => null,
						_ => new League(c.League!.Id, leagueMatches, c.League.QPositions, c.Id, desempateMatchesBuilder.IsEmpty ? null : desempateMatchesBuilder.Build())
					};

					var sf1Matches = sf1MatchesBuilder.Build();
					PlayoffsRound? sf1 = sf1Matches.GetAll().Count switch {
						0 => null,
						_ => new PlayoffsRound(c.Playoffs!.Semifinals1!.Id, c.Playoffs.Semifinals1.Name, sf1Matches)
					};

					var sf2Matches = sf2MatchesBuilder.Build();
					PlayoffsRound? sf2 = sf2Matches.GetAll().Count switch {
						0 => null,
						_ => new PlayoffsRound(c.Playoffs!.Semifinals2!.Id, c.Playoffs.Semifinals2.Name, sf2Matches)
					};

					var finalsMatches = finalsMatchesBuilder.Build();
					PlayoffsRound? finals = finalsMatches.GetAll().Count switch {
						0 => null,
						_ => new PlayoffsRound(c.Playoffs!.Finals!.Id, c.Playoffs.Finals.Name, finalsMatches)
					};

					var playoffs = new Playoffs(c.Playoffs?.Id ?? "", sf1, sf2, finals, c.Playoffs?.DefaultDurations ?? [], league!);

					sf1?.SetParent(playoffs);
					sf2?.SetParent(playoffs);
					finals?.SetParent(playoffs);

					var elos = new List<InitialPlayerElo>();
					foreach (var elo in c.InitialElos) {
						elos.Add(new(instance.Players[elo.PlayerId], elo.Elo));
					}

					instance._competitions[c.Id] = new(c.Id, c.Name, league, playoffs, elos, c.Type);
				}

				foreach (var t in tagDao.GetTagsMap()) {
					if (t.Value.ReplacerType == TagReplacerType.Internal) {
						logger.Warn($"No se ha añadido el replacer interno {t.Key} durante la carga del contexto.");
						continue;
					}

#pragma warning disable CS8509 // La expresión switch no controla todos los valores posibles de su tipo de entrada (no es exhaustiva).
					instance._tagReplacers[t.Key] = t.Value.ReplacerType switch {
						TagReplacerType.File => new Tr_File(t.Value.TagReplacerData, new FileReader()),
						TagReplacerType.Inline => new Tr_Inline(t.Value.TagReplacerData)
					};
#pragma warning restore CS8509
				}

				instance.News = newsDao.GetAllNews();

				// Internal.
				instance._tagReplacers[Tr_HistoricalRankingEntry.Id] = new Tr_HistoricalRankingEntry(new FileReader());
				instance._tagReplacers["cwssg:version"] = new Tr_Inline(Assembly.GetExecutingAssembly().GetName().Version!.ToString());
				instance._tagReplacers["cwssg:league:history"] = new Tr_MatchHistory(new FileReader());
				instance._tagReplacers["cwssg:league:desempate:history"] = new Tr_DesempateMatchHistory(new FileReader());
				instance._tagReplacers["cwssg:league:ranking"] = new Tr_LeagueRanking(new FileReader());
				instance._tagReplacers["cwssg:partidasporjugar"] = new Tr_PorJugar(new FileReader());
				instance._tagReplacers["cwssg:playoffs:phase:history"] = new Tr_PlayoffPhaseHistory(new FileReader());
				instance._tagReplacers["cwssg:competition:stats"] = new Tr_CompetitionStatsTable(new FileReader());
				instance._tagReplacers["cwssg:playoffs:draw:bo5"] = new Tr_PlayoffsDraw_Bo5(new FileReader());
				instance._tagReplacers["cwssg:playoffs:draw:bo7"] = new Tr_PlayoffsDraw_Bo7(new FileReader());
				instance._tagReplacers["cwssg:competition:initialelo:elo"] = new Tr_InitialEloTable(new FileReader());
				instance._tagReplacers["cwssg:elo:rankings"] = new Tr_EloTable(new FileReader());
				instance._tagReplacers["cwssg:header"] = new Tr_Header(new FileReader());
				instance._tagReplacers["cwssg:news:entries"] = new Tr_NewsEntries(new FileReader());
				instance._tagReplacers["cwssg:news:lite:entries"] = new Tr_NewsEntries_Lite(new FileReader());

				logger.Trace($"Carga finalizada.");

				return this;
			}

			/// <summary>
			/// Establece los elos de los jugadores.
			/// </summary>
			/// <param name="elos">ELOs.</param>
			/// <param name="date">Fecha de la última actualización.</param>
			/// <returns></returns>
			public Builder SetElos(List<EloDto> elos, string date) {
				foreach (var elo in elos) {
					PlayerEloEntry? rapid = null;
					PlayerEloEntry? blitz = null;

					if (elo.Rapid != null) {
						rapid = new(elo.Rapid.Elo, elo.Rapid.Wins, elo.Rapid.Draws, elo.Rapid.Losses);
					}

					if (elo.Blitz != null) {
						blitz = new(elo.Blitz.Elo, elo.Blitz.Wins, elo.Blitz.Draws, elo.Blitz.Losses);
					}

					instance._elos[elo.PlayerId] = new(instance.Players[elo.PlayerId], rapid, blitz);
				}

				instance._elosDates = date;

				return this;
			}

			public IReadOnlyDictionary<string, Player> GetCurrentPlayers() => instance.Players;

			/// <returns>Instancia construida.</returns>
			public Context Build() {
				instance.BuildAllMatchesList();
				instance.BuildHistoricalRanking();
				instance.BuildRecords();

				return instance;
			}

			private readonly Context instance = Context.Empty;

		}

		/// <summary>
		/// Construye la lista con todas las partidas de todas
		/// las competiciones.
		/// </summary>
		private void BuildAllMatchesList() {
			var builder = new MatchList.Builder();

			foreach (Competition c in Competitions.Values) {
				if (c.LeaguePhase != null) {
					builder.AddAllMatches(c.LeaguePhase.Matches);
					builder.AddAllMatches(c.LeaguePhase.DesempateMatches ?? new());
				}

				builder.AddAllMatches(c.Playoffs?.Semifinals1?.Matches ?? new());
				builder.AddAllMatches(c.Playoffs?.Semifinals2?.Matches ?? new());
				builder.AddAllMatches(c.Playoffs?.Finals?.Matches ?? new());
			}

			_allMatches = builder.Build();
		}

		/// <summary>
		/// Construye el ranking global.
		/// </summary>
		private void BuildHistoricalRanking() {
			var builder = new HistoricalRanking.Builder();

			foreach (var competition in Competitions.Values) {
				builder.ApplyCompetition(competition);
			}

			_historicalRanking = builder.WithPlayers([.. _players.Values]).Build();
		}

		/// <summary>
		/// Construye los records personales de todos los jugadores.
		/// </summary>
		private void BuildRecords() {
			foreach (var player in Players.Values) {
				_playerRecords[player.Id] = GetPersonalRecords(player);
			}
		}

		/// <summary>
		/// Construye los records de un jugador en concreto.
		/// </summary>
		/// <param name="player">Jugador.</param>
		/// <returns>Records del jugador.</returns>
		private PersonalRecords GetPersonalRecords(Player player) {
			var builder = new PersonalRecords.Builder();

			foreach (var c in Competitions.Values) {

				if (c.LeaguePhase != null) {
					int wins = 0;
					int points = 0;

					foreach (var m in c.LeaguePhase.Matches.GetAll().Where(x => x.First == player || x.Second == player)) {
						if (m.Result == Winner.Draw) {
							builder.AddDraw();
							points++;
							continue;
						}

						if ((m.First == player && m.Result == Winner.First) || (m.Second == player && m.Result == Winner.Second)) {
							builder.AddWin();
							wins++;
							points += 2;
						}
						else {
							builder.AddLoss();
						}
					}

					if (c.LeaguePhase.DesempateMatches != null) {
						foreach (var m in c.LeaguePhase.DesempateMatches.GetAll().Where(x => x.First == player || x.Second == player)) {
							if (m.Result == Winner.Draw) {
								builder.AddDraw();
								points++;
								continue;
							}

							if ((m.First == player && m.Result == Winner.First) || (m.Second == player && m.Result == Winner.Second)) {
								builder.AddWin();
								wins++;
								points += 2;
							}
							else {
								builder.AddLoss();
							}
						}
					}

					var playerInfo = c.LeaguePhase.Ranking.GetPlayerInfo(player);

					if (playerInfo != null) {
						builder.ProcessLeagueWins(playerInfo.Wins, c);
						builder.ProcessLeaguePoints(playerInfo.Points, c);
					}
				}

				if (c.Playoffs != null) {
					if (c.Playoffs.Semifinals1 != null) {
						foreach (var m in c.Playoffs.Semifinals1.Matches.GetAll().Where(x => x.First == player || x.Second == player)) {
							if (m.Result == Winner.Draw) {
								builder.AddDraw();
								continue;
							}

							if ((m.First == player && m.Result == Winner.First) || (m.Second == player && m.Result == Winner.Second)) {
								builder.AddWin();
							}
							else {
								builder.AddLoss();
							}
						}
					}

					if (c.Playoffs.Semifinals2 != null) {
						foreach (var m in c.Playoffs.Semifinals2.Matches.GetAll().Where(x => x.First == player || x.Second == player)) {
							if (m.Result == Winner.Draw) {
								builder.AddDraw();
								continue;
							}

							if ((m.First == player && m.Result == Winner.First) || (m.Second == player && m.Result == Winner.Second)) {
								builder.AddWin();
							}
							else {
								builder.AddLoss();
							}
						}
					}

					if (c.Playoffs.Finals != null) {
						foreach (var m in c.Playoffs.Finals.Matches.GetAll().Where(x => x.First == player || x.Second == player)) {
							if (m.Result == Winner.Draw) {
								builder.AddDraw();
								continue;
							}

							if ((m.First == player && m.Result == Winner.First) || (m.Second == player && m.Result == Winner.Second)) {
								builder.AddWin();
							}
							else {
								builder.AddLoss();
							}
						}
					}
				}
			}

			return builder.Build();
		}

		/// <param name="id">ID de la liga.</param>
		/// <returns>Liga, o null si no existe.</returns>
		public League? GetLeague(string id) {
			foreach (var (_, c) in Competitions) {
				if (c.LeaguePhase?.Id == id) {
					return c.LeaguePhase;
				}
			}

			return null;
		}

		/// <param name="id">ID de la competición.</param>
		/// <returns>Competición, o null si no existe.</returns>
		public Competition? GetCompetition(string id) => Competitions.Values.FirstOrDefault(x => x.Id == id);

		/// <param name="id">ID de los playoffs</param>
		/// <returns>Playoffs, o null si no existe.</returns>
		public Playoffs? GetPlayoffs(string id) => Competitions.Values.FirstOrDefault(x => x.Playoffs?.Id == id)?.Playoffs;

		/// <param name="id">ID de la ronda de playoffs.</param>
		/// <returns>Ronda de playoffs, o null si no existe.</returns>
		public PlayoffsRound? GetPlayoffsRound(string id) 
			=> Competitions.Values.FirstOrDefault(x => x.Playoffs?.GetPhase(id) != null)?.Playoffs?.GetPhase(id);


		public List<NewsDto> News { get; private set; } = new();

		private MatchList _allMatches = new();
		public MatchList AllMatches { get => _allMatches; }

		private HistoricalRanking _historicalRanking = new();
		public HistoricalRanking HistoricalRanking { get => _historicalRanking; }

		private string _elosDates = string.Empty;
		public string ElosDate {  get => _elosDates; }

		private readonly Dictionary<string, PlayerElo> _elos = [];
		public IReadOnlyDictionary<string, PlayerElo> Elos { get => _elos; }

		private readonly Dictionary<string, PersonalRecords> _playerRecords = [];
		public IReadOnlyDictionary<string, PersonalRecords> PlayerRecords { get => _playerRecords; }

		private readonly Dictionary<string, Country> _countries = [];
		public IReadOnlyDictionary<string, Country> Countries => _countries;

		private readonly Dictionary<string, Player> _players = [];
		public IReadOnlyDictionary<string, Player> Players { get => _players; }

		private readonly Dictionary<string, ITagReplacer> _tagReplacers = [];
		public IReadOnlyDictionary<string, ITagReplacer> TagsReplacers { get => _tagReplacers; }

		private readonly Dictionary<string, Competition> _competitions = [];
		public IReadOnlyDictionary<string, Competition> Competitions { get => _competitions; }

		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

	}

}
