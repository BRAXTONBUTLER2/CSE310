import arcade
import random

SCREEN_WIDTH = 600
SCREEN_HEIGHT = 800
SCREEN_TITLE = "Flappy Bird Custom"

BIRD_START_X = 100
BIRD_START_Y = SCREEN_HEIGHT // 2
GRAVITY = 0.5
FLAP_STRENGTH = 10

PIPE_WIDTH = 80     # Width of your pipe sprite
PIPE_GAP = 200
PIPE_SPEED = 3

class Bird(arcade.Sprite):
    def __init__(self):
        super().__init__("bird.png", 0.5)
        self.center_x = BIRD_START_X
        self.center_y = BIRD_START_Y
        self.change_y = 0

    def update(self):
        self.change_y -= GRAVITY
        self.center_y += self.change_y

        if self.center_y < 0:
            self.center_y = 0
            self.change_y = 0
        if self.center_y > SCREEN_HEIGHT:
            self.center_y = SCREEN_HEIGHT
            self.change_y = 0

class Pipe(arcade.Sprite):
    def __init__(self, x, y, is_top):
        super().__init__("pipe.png", 0.8)
        self.center_x = x
        self.center_y = y
        self.is_top = is_top
        if is_top:
            self.angle = 180

    def update(self):
        self.center_x -= PIPE_SPEED

class Button:
    """Simple clickable button."""
    def __init__(self, center_x, center_y, width, height, text):
        self.center_x = center_x
        self.center_y = center_y
        self.width = width
        self.height = height
        self.text = text

    def draw(self):
        arcade.draw_rectangle_filled(self.center_x, self.center_y, self.width, self.height, arcade.color.DARK_BLUE)
        arcade.draw_text(self.text, self.center_x, self.center_y, arcade.color.WHITE, 24, anchor_x="center", anchor_y="center")

    def check_click(self, x, y):
        return (self.center_x - self.width / 2 < x < self.center_x + self.width / 2 and
                self.center_y - self.height / 2 < y < self.center_y + self.height / 2)

class FlappyBirdGame(arcade.Window):
    def __init__(self):
        super().__init__(SCREEN_WIDTH, SCREEN_HEIGHT, SCREEN_TITLE)
        self.background = arcade.load_texture("background.png")
        self.bird = None
        self.pipes = []
        self.score = 0
        self.game_over = False
        self.spawn_timer = 0

        self.flap_sound = arcade.load_sound("flap.wav")
        self.hit_sound = arcade.load_sound("hit.wav")
        self.point_sound = arcade.load_sound("point.wav")

        self.retry_button = Button(SCREEN_WIDTH // 2, SCREEN_HEIGHT // 2 - 100, 200, 60, "Retry")

    def setup(self):
        self.bird = Bird()
        self.pipes = []
        self.score = 0
        self.game_over = False
        self.spawn_timer = 0

        # Spawn first pipes just offscreen right
        self.spawn_pipes(initial=True)

    def spawn_pipes(self, initial=False):
        gap_center = random.randint(PIPE_GAP, SCREEN_HEIGHT - PIPE_GAP)
        top_y = gap_center + PIPE_GAP // 2
        bottom_y = gap_center - PIPE_GAP // 2

        if initial:
            pipe_x = SCREEN_WIDTH + PIPE_WIDTH // 2
        else:
            if self.pipes:
                last_pipe_x = max(pipe.center_x for pipe in self.pipes)
                pipe_x = last_pipe_x + 300
            else:
                pipe_x = SCREEN_WIDTH + PIPE_WIDTH // 2

        top_pipe = Pipe(pipe_x, top_y, is_top=True)
        bottom_pipe = Pipe(pipe_x, bottom_y, is_top=False)

        self.pipes.append(top_pipe)
        self.pipes.append(bottom_pipe)

    def on_draw(self):
        arcade.start_render()
        arcade.draw_lrwh_rectangle_textured(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT, self.background)
        self.bird.draw()
        for pipe in self.pipes:
            pipe.draw()

        arcade.draw_text(f"Score: {self.score}", 10, SCREEN_HEIGHT - 40, arcade.color.BLACK, 24)

        if self.game_over:
            arcade.draw_rectangle_filled(SCREEN_WIDTH // 2, SCREEN_HEIGHT // 2, SCREEN_WIDTH, SCREEN_HEIGHT, (0, 0, 0, 150))
            arcade.draw_text("GAME OVER", SCREEN_WIDTH // 2, SCREEN_HEIGHT // 2 + 50,
                             arcade.color.RED, 50, anchor_x="center")
            self.retry_button.draw()

    def on_update(self, delta_time):
        if self.game_over:
            return

        self.bird.update()
        for pipe in self.pipes:
            pipe.update()

        self.pipes = [pipe for pipe in self.pipes if pipe.center_x + pipe.width / 2 > 0]

        if not self.pipes or max(pipe.center_x for pipe in self.pipes) < SCREEN_WIDTH:
            self.spawn_pipes()

        # Increase score when bird passes bottom pipe
        for pipe in self.pipes:
            if not hasattr(pipe, 'passed') and pipe.center_x + pipe.width / 2 < self.bird.center_x:
                pipe.passed = True
                if pipe.is_top is False:
                    self.score += 1
                    arcade.play_sound(self.point_sound)

        for pipe in self.pipes:
            if arcade.check_for_collision(self.bird, pipe):
                self.game_over = True
                arcade.play_sound(self.hit_sound)
                break

        if self.bird.center_y <= 0 or self.bird.center_y >= SCREEN_HEIGHT:
            self.game_over = True
            arcade.play_sound(self.hit_sound)

    def on_key_press(self, key, modifiers):
        if key == arcade.key.SPACE and not self.game_over:
            self.bird.change_y = FLAP_STRENGTH
            arcade.play_sound(self.flap_sound)

    def on_mouse_press(self, x, y, button, modifiers):
        if self.game_over and self.retry_button.check_click(x, y):
            self.setup()

if __name__ == "__main__":
    game = FlappyBirdGame()
    game.setup()
    arcade.run()
