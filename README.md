# Saving Time

A tool to store time records at work

# Prerequisites
- net6.0
- Database Server (example MSSQLLocalDB)

# Installation
```
> dotnet build
```

# Usage
```
> dotnet run --project SavingTime/SavingTime.csproj
```

# Testing
```
> dotnet test
```

# TODO
- [x] Add REPL or args parser lib
- [x] Create repository
- [x] Entry
- [x] Exit
- [x] Set DateTime
- [x] Set Time
- [x] Help
- [x] History
- [x] Context
- [x] Add context to time record
- [x] Integrate with Lacuna Web App for log work
- [x] Add a generic command to decide if it is entry or exit
- [x] After register a time show the time and some information about that day
- [x] Add a feature to track issues time spent
- [x] Add command to view total hours by day in summary view
- [x] Add diff on summary view (-8h + totalhours)
- [ ] Choose one day in info command
- [ ] Show issue info in info command
- [x] Show Issue Summary separated from Time Records Summary
- [ ] Sum Total Diff and Total hours of the month or period
- [x] Make current month as default on summary
- [ ] Make current month as default on summary issue
- [ ] Choose one month on summary 
- [ ] Add a issue summary ordered by issue (not days)
- [ ] Show every day on summary (remove diff from weekend and holidays)

- [ ] Add version per build (to see if the deploy was done successfully)
- [ ] Add balance per month / week / full

- [ ] Internalize database with SqLite to install db with the application
- [ ] Setup Instalation for the console application on windows
- [ ] Unit Tests
- [ ] Id to GUID type
- [ ] Better error handling
