// *******************************
// *******************************
// ENSURE TO SET THE DB NAME HERE.
// BE CAREFUL AND SELECT THE CORRECT DB

var dbName = "Leafsteroids";
//var dbName = "Leafsteroids_stage";

// *******************************
// *******************************

// Re-create player_unique collection, now with range sharding on _id
// _id will store the Nickname ensuring it is unique and avoids an extra index
 
db.players_unique.drop();
sh.shardCollection(dbName+".players_unique", { _id: 1 }, true);

db.players_unique.createIndex({ _id: 1, location: 1 }); // for covered queries

// Pipeline
var pipeline = [
    {
        $project: {
            "_id": "$Nickname",
            "location": "$location",
        },
    },
    {
        $merge: {
            into: "players_unique"
        },
    },
];

db.players.aggregate(pipeline);

// Ensure same count
db.players.countDocuments();
db.players_unique.countDocuments();