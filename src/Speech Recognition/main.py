from flask import Flask, request, jsonify
from huggingsound import SpeechRecognitionModel

app = Flask(__name__)

model = SpeechRecognitionModel("jonatasgrosman/wav2vec2-large-xlsr-53-russian")

@app.route('/transcribe_audio', methods=['POST'])
def transcribe_audio():
    try:
        # Получаем файл mp3 из запроса
        audio_file = request.files['audio']

        # Сохраняем файл временно (может потребоваться изменить путь)
        audio_path = "temp_audio.mp3"
        audio_file.save(audio_path)

        # Передаем файл в модель распознавания голоса
        transcriptions = model.transcribe([audio_path])

        text = transcriptions[0]['transcription']

        # Возвращаем результат в ответ
        return jsonify({'status': 'success', 'transcriptions': text})

    except Exception as e:
        return jsonify({'status': 'error', 'message': str(e)})


if __name__ == '__main__':
    app.run(debug=True, port=5000)
