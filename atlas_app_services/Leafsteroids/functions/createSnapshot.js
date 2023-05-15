exports = async function({ query, headers, body}, response) {
    try {
      let snapshot = JSON.parse(body.text());
      snapshot.DateTime = new Date();
      const result = await context.services.get("mongodb-atlas").db("Leafsteroids").collection("recordings").insertOne(snapshot);
      return result;
      
    } catch(error) {
      console.log(JSON.stringify(error));
    }
};