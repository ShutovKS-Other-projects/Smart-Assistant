import torch
import emoji
from flask import Flask, request, jsonify
from transformers import AutoTokenizer, AutoModelWithLMHead

app = Flask(__name__)

# Загрузка модели и токенизатора
tokenizer = AutoTokenizer.from_pretrained('tinkoff-ai/ruDialoGPT-medium')
model = AutoModelWithLMHead.from_pretrained('tinkoff-ai/ruDialoGPT-medium')


def extract_messages(input_string):
    messages = []

    # Разделяем строку по @@
    segments = input_string.split('@@')

    # Проходим по каждому сегменту, начиная со второго (первый сегмент - пустая строка)
    for i in range(1, len(segments), 2):
        recipient = segments[i].strip()

        message = segments[i + 1].strip().rstrip() if i + 1 < len(segments) else None

        if message is None or len(message) == 0:
            continue

        message_no_emoji = ''.join(char for char in message if char not in emoji.EMOJI_DATA)

        messages.append((recipient, message_no_emoji))

    return messages


@app.route('/generate_response', methods=['POST'])
def generate_response():
    try:
        user_input = request.form['user_input']

        # Токенизация входных данных
        inputs = tokenizer(user_input, return_tensors='pt')

        # Генерация ответа
        generated_token_ids = model.generate(
            **inputs,
            top_k=10,
            top_p=0.95,
            num_beams=3,
            num_return_sequences=1,
            do_sample=True,
            no_repeat_ngram_size=2,
            temperature=1.2,
            repetition_penalty=1.2,
            length_penalty=1.0,
            eos_token_id=50257,
            max_new_tokens=40
        )

        generated_response = tokenizer.decode(generated_token_ids[0], skip_special_tokens=True)

        messages = extract_messages(generated_response)
        last_message = messages[-1][1] if messages else None

        return jsonify({'status': 'success', 'generated_response': last_message})

    except Exception as e:
        return jsonify({'status': 'error', 'message': str(e)})


if __name__ == '__main__':
    app.run(debug=True, port=5002)
