module LogLevels

let message (logLine: string): string =
  logLine.Split("]:").[1].Trim()

let logLevel(logLine: string): string = 
  logLine.Substring(1).Split("]").[0].ToLower()

let reformat(logLine: string): string = 
  $"{message logLine} ({logLevel logLine})"