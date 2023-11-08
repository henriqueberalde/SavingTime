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
- [ ] Add command to view total hours by day (query already created) or put it inside the summary (round total day on removing the seconds)
- [ ] Add a feature to track issues time spent
- [ ] Internalize database with SqLite to install db with the application
- [ ] Setup Instalation for the console application on windows
- [ ] Unit Tests
- [ ] History per date range
- [ ] History per context
- [ ] Balance
- [ ] History with balance
- [ ] Id to GUID type
- [ ] Error handling
