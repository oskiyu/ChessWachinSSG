// See https://aka.ms/new-console-template for more information
using ChessWachinSSG;

using NLog;
using NLog.Conditions;
using NLog.Targets;

var consoleTarget = new ColoredConsoleTarget();

var highlightRule = new ConsoleRowHighlightingRule();
var highlightRule2 = new ConsoleRowHighlightingRule();
highlightRule.Condition = ConditionParser.ParseExpression("level == LogLevel.Info");
highlightRule.ForegroundColor = ConsoleOutputColor.Green;
highlightRule2.Condition = ConditionParser.ParseExpression("level == LogLevel.Error");
highlightRule2.ForegroundColor = ConsoleOutputColor.Red;

consoleTarget.RowHighlightingRules.Add(highlightRule);
consoleTarget.RowHighlightingRules.Add(highlightRule2);

LogManager.Setup().LoadConfiguration(builder => {
	builder.ForLogger().FilterMinLevel(LogLevel.Trace).WriteTo(consoleTarget);
});

Main.Run(args);