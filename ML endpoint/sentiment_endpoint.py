from flask import Flask, request, jsonify
from textblob import TextBlob

app = Flask(__name__)

from flask_cors import CORS

CORS(app)

@app.route('/api/sentiment', methods=['POST'])
def get_sentiment():
    # Get the sentence from the POST request
    data = request.json
    sentence = data.get("Text", "")

    # Get the sentiment polarity
    analysis = TextBlob(sentence)
    polarity = analysis.sentiment.polarity

    # Convert polarity (-1 to 1) to a score (0 to 1)
    score = (polarity + 1) / 2

    return jsonify({"sentiment_score": score})


if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0', port=7890)
