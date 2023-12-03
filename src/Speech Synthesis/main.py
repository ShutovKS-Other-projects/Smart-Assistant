from flask import Flask, request, jsonify
import pyttsx3
import tempfile

app = Flask(__name__)


def text_to_speech(text, language='ru', rate=150, volume=1.0):
    engine = pyttsx3.init()

    # Устанавливаем язык
    engine.setProperty('voice', f'{language}')

    # Устанавливаем скорость и громкость
    engine.setProperty('rate', rate)
    engine.setProperty('volume', volume)

    # Генерируем временный файл для сохранения аудио
    temp_audio_file = tempfile.NamedTemporaryFile(suffix='.mp3', delete=False)
    temp_audio_file_path = temp_audio_file.name
    temp_audio_file.close()

    # Сохраняем аудиофайл
    engine.save_to_file(text, temp_audio_file_path)

    # Запускаем процесс генерации аудио
    engine.runAndWait()

    return temp_audio_file_path


@app.route('/text_to_speech', methods=['POST'])
def process_text_to_speech():
    try:
        text = request.form['text']
        language = request.form['language'] if 'language' in request.files else 'ru'
        rate = request.form['rate'] if 'rate' in request.files else 150
        volume = request.form['volume'] if 'volume' in request.files else 1.0

        # Преобразуем текст в аудио
        audio_file_path = text_to_speech(text, language, rate, volume)

        # Отправляем аудиофайл в ответ
        return jsonify({'status': 'success', 'audio_path': audio_file_path})

    except Exception as e:
        return jsonify({'status': 'error', 'message': str(e)})


if __name__ == '__main__':
    app.run(debug=True, port=5001)
