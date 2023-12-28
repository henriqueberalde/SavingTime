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
- [ ] Choose one day in Info command
- [ ] Show Issue Summary separated from Time Records Summary
- [ ] Sum Total Diff and Total hours of the month
- [ ] Choose one month on summary Time Records and Issue (make current month as default)

- [ ] Add version per build (to see if the deploy was done successfully)
- [ ] Add balance per month / week / full

- [ ] Add command to view total hours by day (query already created) or put it inside the summary (round total day on removing the seconds)
- [ ] Internalize database with SqLite to install db with the application
- [ ] Setup Instalation for the console application on windows
- [ ] Unit Tests
- [ ] History per date range
- [ ] History per context
- [ ] Balance
- [ ] History with balance
- [ ] Id to GUID type
- [ ] Error handling
