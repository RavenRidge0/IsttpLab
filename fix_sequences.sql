SELECT setval(pg_get_serial_sequence('"Cars"', 'Id'), COALESCE(MAX("Id"), 0) + 1, false) FROM "Cars";
SELECT setval(pg_get_serial_sequence('"Brands"', 'Id'), COALESCE(MAX("Id"), 0) + 1, false) FROM "Brands";
SELECT 'Sequences fixed!' as result;
