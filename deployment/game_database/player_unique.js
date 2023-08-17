// Re-create player_unique collection, now with range sharding on _id
db.players_unique.drop({});
sh.shardCollection("Leafsteroids.players_unique", { _id: 1 }, true, 16)

// Pipeline
var pipeline = [
    {
        $project: {
            "_id.Nickname": "$Nickname",
            "_id.location": "$location",
        },
    },
    {
        $out: {
            db: "Leafsteroids",
            coll: "players_unique",
        },
    },
];

db.players.aggregate(pipeline);

// Ensure same count
db.players.countDocuments();
db.players_unique.countDocuments();