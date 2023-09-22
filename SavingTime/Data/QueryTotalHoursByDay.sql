select
	record_date,
	ROUND(SUM(total), 2) as total_decimal,
	CAST(CONVERT(VARCHAR,DATEADD(SECOND, ROUND(SUM(total), 2) * 3600, 0),108) AS TIME) as total_time,
	ROUND(SUM(total)-8, 2) as debt_decimal,
	CAST(CONVERT(VARCHAR,DATEADD(SECOND, ABS(SUM(total)-8) * 3600, 0),108) AS TIME) as debt_time
FROM (
	select
	convert(varchar, Time, 111) as record_date,
	CASE WHEN Type = 1
		THEN (DATEPART(HOUR, Time)+ROUND(CAST(DATEPART(MINUTE, Time) as float)/CAST(60 as float), 2)) * -1
		ELSE DATEPART(HOUR, Time)+ROUND(CAST(DATEPART(MINUTE, Time) as float)/CAST(60 as float), 2)
	END AS total
	from TimeRecords
) as t GROUP BY record_date
