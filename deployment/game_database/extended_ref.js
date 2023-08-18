// find offending recordings
db.recordings.countDocuments({ "Player.location": { $exists: false } });

// Ensure every player in a recording has their location stored
db.players.find({ location: { $exists: true } }).forEach(function (player) {
    print("Updating " + player.Nickname + "'s location of " + player.location + " into recordings");
    db.recordings.updateMany(
        { "Player.Nickname": player.Nickname },
        {
            $set: {
                "Player.location": player.location
            }
        }
    );
});

// Ensure all players in recordings have a location. This should equate to 0
db.recordings.countDocuments({ "Player.location": { $exists: false } });

// Clean-up old and test data
db.recordings.deleteMany({"location": {$in: [null, '', "TESTING"]}});

// Unique index required for $merge 
db.recordings.createIndex({"location": 1, "Player.Nickname": 1, "_id": 1}, {unique: true});

// UPDATE
var pipeline = [
    {
        "$set": {
            "DateTime": {$toDate: "$DateTime"}
        },
    },
    {
        "$unset": [
            "Event.location",
            "Event.name",
            "Player._id",
            "Player.Email",
            "Player.TeamName",
        ]
    },
    {
        "$merge": {
            "into": "recordings",
            "on": ["location", "Player.Nickname", "_id"],
            "whenMatched": "merge"
        }
    }
];

// Perform update on all recordings
db.recordings.aggregate(pipeline);

// Verify data. Count should be zero
var query = {
    "$or":
        [
            {"Player.location": { $exists: false } },
            {"Event.location": { $exists: true } },
            {"Event.name": { $exists: true } },
            {"Player._id": { $exists: true } },
            {"Player.Email": {$exists: true}},
            {"Player.TeamName": {$exists: true}},
            {"DateTime": {$not: {$type: 9}}},
            {"location": {$in: [null, '', "TESTING"]}}
        ]
};
db.recordings.countDocuments(query);


