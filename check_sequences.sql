-- Показати всі послідовності
SELECT sequence_name FROM information_schema.sequences;

-- Показати поточні максимальні ID
SELECT 'Cars max Id' as tbl, COALESCE(MAX("Id"), 0) as max_id FROM "Cars"
UNION ALL
SELECT 'Brands max Id', COALESCE(MAX("Id"), 0) FROM "Brands";
