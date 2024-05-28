SELECT
    [Id],
    [Key],
    [Value],
    [Type]
FROM SystemParameter
WHERE [Key] = @Key