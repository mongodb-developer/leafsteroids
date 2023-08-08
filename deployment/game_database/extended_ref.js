// Clean-up old and test data
db.recordings.deleteMany({"location": {$in: [null, '', "TESTING"]}});

// Unique index required for $merge 
db.recordings.createIndex({"location": 1, "Player.Nickname": 1, "_id": 1}, {unique: true});

// UPDATE
var pipeline = [
    {
        "$set": {
            "DateTime": {$toDate: "$DateTime"},
            "Player._id": {$toObjectId: "$Player._id"},
        },
    },
    {
        "$unset": [
            "Event.location",
            "Event.name",
            "Player.Email",
            "Player.TeamName"
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
            {"Event.location": {$exists: true}},
            {"Event.name": {$exists: true}},
            {"Player.Email": {$exists: true}},
            {"Player.TeamName": {$exists: true}},
            {"DateTime": {$not: {$type: 9}}},
            {"Player._id": {$not: {$type: 7}}},
            {"location": {$in: [null, '', "TESTING"]}}
        ]
};
db.recordings.countDocuments(query);