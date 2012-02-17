using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OcarinaHeroLibrary.Controller;
using OcarinaHeroLibrary.Songs;
using OcarinaHeroLibrary.Graphics;

namespace OcarinaHeroLibrary.Screens
{
    public delegate void EmptyArgsHandler();
    public class SongSelectionScreen : IScreen
    {
        protected List<SongSelectionListItem> list = new List<SongSelectionListItem>();
        private int listOffset = 0;
        private const int listDisplayLength = 20;
        protected BaseSong[] songs;
        protected const int songListWidth = 300;
        protected const int songListHeaderHeight = 100;
        protected static Time keyDownRepeat = new Time(0, 0, 250);
        protected SpriteFont font;
        protected ApplicationSkin skin;
        protected EngineModel track;
        protected EngineCamera camera;
        protected BasicEffect effect;
        protected bool viewportGoingLeft = true;
        protected SongSelectionScreenSpecialButton[] specialButtons;
        protected SongSelectionSelection selection = new SongSelectionSelection();

        protected float skyBackgroundX;

        protected int songSelectionIndex;

        public SongSelectionScreen(BaseSong[] songList, ApplicationSkin applicationSkin,
            GraphicsDevice graphicsDevice)
        {
            Nullable<SongDifficulty> previousDifficulty = null;
            List<BaseSong> orderedSongs = (songList.OrderBy(item => item.Difficulty)).ToList();
            foreach (BaseSong song in orderedSongs)
            {
                if (song.Difficulty != previousDifficulty)
                {
                    list.Add(new SongSelectionListItem(null, song.Difficulty.ToString(), false, true, 5));
                    previousDifficulty = song.Difficulty;
                }
                list.Add(new SongSelectionListItem(song, song.Title, true, true, 25));
            }
            selection.SelectionIndex = 1;
            skin = applicationSkin;
            track = skin.Ocarina;
            camera = new EngineCamera(graphicsDevice.Viewport);
            track.RotationX += MathHelper.PiOver2;
            track.RotationY += MathHelper.Pi;
            effect = new BasicEffect(graphicsDevice, new EffectPool());
            downKeyPressedDuration = null;
            upKeyPressedDuration = null;
            leftKeyPressedDuration = null;
            rightKeyPressedDuration = null;
            specialButtons = new SongSelectionScreenSpecialButton[2];
            specialButtons[0] = new SongSelectionScreenSpecialButton("Settings");
            specialButtons[0].Selected += delegate()
            {
                if (OpenScreen != null)
                    OpenScreen.Invoke(new SettingsScreen());
            };
            specialButtons[1] = new SongSelectionScreenSpecialButton("Exit");
            specialButtons[1].Selected += delegate()
            {
                if (ExitGame != null)
                    ExitGame.Invoke();
            };
        }
        public event EmptyArgsHandler ExitGame;
        protected const float TrackRotationSpeed = (MathHelper.Pi / 300);
        protected Time downKeyPressedDuration, upKeyPressedDuration, leftKeyPressedDuration,
            rightKeyPressedDuration;
        public void Update(GameTime gameTime, IController controller, ref Gamer gamer)
        {
            ControllerInput input = controller.GetInput(gameTime.ElapsedGameTime);

            bool processUp, processDown, processLeft, processRight;
            processUp = (input.Up && (upKeyPressedDuration == null ||
                upKeyPressedDuration + gameTime.ElapsedGameTime >= keyDownRepeat));
            if (input.Up)
                upKeyPressedDuration = (upKeyPressedDuration == null ? new Time(0, 0, 0) :
                    upKeyPressedDuration + gameTime.ElapsedGameTime);
            else if (!input.Up && upKeyPressedDuration != null)
                upKeyPressedDuration = null;
            processDown = (input.Down && (downKeyPressedDuration == null ||
                downKeyPressedDuration + gameTime.ElapsedGameTime >= keyDownRepeat));
            if (input.Down)
                downKeyPressedDuration = (downKeyPressedDuration == null ? new Time(0, 0, 0) :
                    downKeyPressedDuration + gameTime.ElapsedGameTime);
            else if (!input.Down && downKeyPressedDuration != null)
                downKeyPressedDuration = null;
            processLeft = (input.Left && (leftKeyPressedDuration == null ||
                leftKeyPressedDuration + gameTime.ElapsedGameTime >= keyDownRepeat));
            if (input.Left)
                leftKeyPressedDuration = (leftKeyPressedDuration == null ? new Time(0, 0, 0) :
                    leftKeyPressedDuration + gameTime.ElapsedGameTime);
            else if (!input.Left && leftKeyPressedDuration != null)
                leftKeyPressedDuration = null;
            processRight = (input.Right && (rightKeyPressedDuration == null ||
                rightKeyPressedDuration + gameTime.ElapsedGameTime >= keyDownRepeat));
            if (input.Right)
                rightKeyPressedDuration = (rightKeyPressedDuration == null ? new Time(0, 0, 0) :
                    rightKeyPressedDuration + gameTime.ElapsedGameTime);
            else if (!input.Right && rightKeyPressedDuration != null)
                rightKeyPressedDuration = null;
            if (upKeyPressedDuration != null)
               while (upKeyPressedDuration >= keyDownRepeat)
                    upKeyPressedDuration -= keyDownRepeat;
            if (downKeyPressedDuration != null)
                while (downKeyPressedDuration >= keyDownRepeat)
                    downKeyPressedDuration -= keyDownRepeat;
            if (leftKeyPressedDuration != null)
                while (leftKeyPressedDuration >= keyDownRepeat)
                    leftKeyPressedDuration -= keyDownRepeat;
            if (rightKeyPressedDuration != null)
                while (rightKeyPressedDuration >= keyDownRepeat)
                    rightKeyPressedDuration -= keyDownRepeat;

            if (selection.InSpecialButtons)
            {
                int newTravel = 0;
                if (processLeft)
                    newTravel--;
                if (processRight)
                    newTravel++;
                if (processDown || processUp)
                {
                    int validItem = (processDown ? list.FindIndex(item => item.Highlightable && item.Enabled) :
                        list.FindLastIndex(item => item.Highlightable && item.Enabled));
                    listOffset = (validItem - listDisplayLength);
                    if (listOffset < 0)
                        listOffset = 0;
                    selection.InSpecialButtons = false;
                    selection.InSongList = true;
                    selection.SelectionIndex = validItem - listOffset;
                }
                else if (selection.SelectionIndex + newTravel < 0)
                {
                    selection.SelectionIndex = specialButtons.Length - 1;
                }
                else if (selection.SelectionIndex + newTravel >= specialButtons.Length)
                {
                    selection.SelectionIndex = 0;
                }
                else
                    selection.SelectionIndex += newTravel;
            }
            else
            {
                int newTravel = 0;
                if (processDown)
                    newTravel++;
                if (processUp)
                    newTravel--;
                if (processLeft || processRight)
                {
                    selection.InSpecialButtons = true;
                    selection.InSongList = false;
                    selection.SelectionIndex = 0;
                }
                else if (newTravel != 0)
                {
                    int highestListOffset = list.Count - listDisplayLength;
                    if (highestListOffset < 0)
                        highestListOffset = 0;
                    int newListOffset = listOffset;
                    int newSelectionIndex = selection.SelectionIndex;
                    do
                    {
                        newSelectionIndex += newTravel;
                        if (newSelectionIndex < 0 && newListOffset > 0)
                        {
                            newListOffset += newSelectionIndex;
                            newSelectionIndex = 0;
                        }
                        if (newSelectionIndex >= listDisplayLength && newListOffset + newSelectionIndex < list.Count)
                        {
                            newListOffset += (listDisplayLength - newSelectionIndex);
                            newSelectionIndex = listDisplayLength;
                        }
                        if (newSelectionIndex < 0 && newListOffset <= 0)
                        {
                            newSelectionIndex = (list.Count > listDisplayLength ? listDisplayLength : list.Count) - 1;
                            newListOffset = list.Count - listDisplayLength;
                            if (newListOffset < 0)
                                newListOffset = 0;
                        }
                        if (newSelectionIndex + newListOffset >= list.Count)
                        {
                            newSelectionIndex = 0;
                            newListOffset = 0;
                        }
                    } while (!list[newListOffset + newSelectionIndex].Enabled ||
                        !list[newListOffset + newSelectionIndex].Highlightable);
                    listOffset = newListOffset;
                    selection.SelectionIndex = newSelectionIndex;
                }
            }

            if (input.Enter)
            {
                if (selection.InSongList)
                    SongSelected.Invoke(new SongSelectedArgs((BaseSong)list[selection.SelectionIndex].ItemBehind));
                else
                    specialButtons[selection.SelectionIndex].Select();
            }
            track.RotationY += MathHelper.Pi / 200f;
            skyBackgroundX += 1;
            if (skyBackgroundX >= skin.SkyTexture.Width)
                skyBackgroundX = 0;
        }
        public void Draw(GameTime gameTime, Gamer gamer, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            Rectangle songListRectangle = new Rectangle(graphics.GraphicsDevice.Viewport.Width - songListWidth,
                0, songListWidth, graphics.GraphicsDevice.Viewport.Height);
            graphics.GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            Rectangle imageSpace = new Rectangle(0, 0, songListRectangle.X, GlobalConstants.ScreenHeight);
            spriteBatch.Draw(skin.SkyTexture, new Rectangle(0, 0,
                (imageSpace.Width > skin.SkyTexture.Width - skyBackgroundX ?
                skin.SkyTexture.Width - (int)skyBackgroundX : imageSpace.Width), imageSpace.Height),
                new Rectangle((int)skyBackgroundX, 0,
                (imageSpace.Width > skin.SkyTexture.Width - skyBackgroundX ?
                skin.SkyTexture.Width - (int)skyBackgroundX :
                imageSpace.Width), imageSpace.Height), Color.White);
            if (imageSpace.Width > skin.SkyTexture.Width - skyBackgroundX)
            {
                spriteBatch.Draw(skin.SkyTexture, new Rectangle(skin.SkyTexture.Width - (int)skyBackgroundX,
                    0, imageSpace.Width - (skin.SkyTexture.Width - (int)skyBackgroundX), imageSpace.Height),
                    new Rectangle(0, 0, imageSpace.Width - (skin.SkyTexture.Width - (int)skyBackgroundX),
                        imageSpace.Height), Color.White);
            }


            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;
            effect.Begin();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Begin();
                track.Draw(graphics.GraphicsDevice, camera);
                pass.End();
            }
            effect.End();

            spriteBatch.FillRectangle(songListRectangle, Color.White);
            spriteBatch.DrawLine(songListRectangle.X, songListRectangle.Y, songListRectangle.X,
                songListRectangle.Bottom, Color.DarkGray);
            spriteBatch.DrawLine(songListRectangle.X + 10, songListRectangle.Y + songListHeaderHeight,
                songListRectangle.Right - 10, songListRectangle.Y + songListHeaderHeight, Color.Black);

            for (int index = 0; index < list.Count; index++)
            {
                string title = list[index + listOffset].Text;
                Vector2 titleSize = skin.Font.MeasureString(title);//songs[index].Title);
                if (index == selection.SelectionIndex && selection.InSongList)
                {
                    Rectangle selectionRectangle = new Rectangle(songListRectangle.X + 10,
                        songListRectangle.Y + songListHeaderHeight + 5 + 20 * index, songListWidth - 20,
                        20);
                    spriteBatch.FillRectangle(selectionRectangle, Color.LightBlue);
                    spriteBatch.DrawRectangle(selectionRectangle, Color.Black);
                }
                spriteBatch.DrawString(skin.Font, title, //songs[index].Title,
                    new Vector2(songListRectangle.X + list[index + listOffset].Indentation, //(int)((songListWidth - 20 - titleSize.X) / 2),
                        songListRectangle.Y + songListHeaderHeight + 2 + 20 * index), Color.Black);
            }
            Vector2 initialSpecialPosition = new Vector2((songListWidth - specialButtons.Length *
                SongSelectionScreenSpecialButton.Size.X - (specialButtons.Length - 1 > 0 ? specialButtons.Length - 1 :
                0) * SongSelectionScreenSpecialButton.Padding) / 2 + songListRectangle.X,
                graphics.GraphicsDevice.Viewport.Height - 10 - SongSelectionScreenSpecialButton.Size.Y);
            for (int index = 0; index < specialButtons.Length; index++)
            {
                Vector2 position = initialSpecialPosition +
                    new Vector2(index * SongSelectionScreenSpecialButton.Size.X + index *
                        SongSelectionScreenSpecialButton.Padding, 0);
                spriteBatch.FillRectangle(position, SongSelectionScreenSpecialButton.Size,
                    (index == selection.SelectionIndex && selection.InSpecialButtons ? Color.LightGray :
                    Color.Gray));
                spriteBatch.DrawRectangle(position, SongSelectionScreenSpecialButton.Size, Color.Black);
                spriteBatch.DrawString(skin.Font, specialButtons[index].Text, position, Color.Black);
            }

            /* Current Profile Display */
            spriteBatch.FillRectangle(10, 10, 200, 100, Color.White);
            spriteBatch.DrawRectangle(10, 10, 200, 100, Color.Black);
            spriteBatch.DrawString(skin.Font, gamer.Name, new Vector2(13, 13), Color.Red);

            spriteBatch.End();
        }
        public delegate void SongSelectedEventHandler(SongSelectedArgs args);
        public event SongSelectedEventHandler SongSelected;
        public event Screens.OpenScreenHandler OpenScreen;
    }
    public class SongSelectedArgs : EventArgs
    {
        protected BaseSong _song;
        public SongSelectedArgs(BaseSong song)
        {
            _song = song;
        }
        public BaseSong Song { get { return _song; } }
    }
    public class SongSelectionScreenSpecialButton
    {
        public static Vector2 Size = new Vector2(60, 60);
        public const int Padding = 10;
        protected string text;
        public string Text { get { return text; } }
        public SongSelectionScreenSpecialButton(string label)
        {
            text = label;
        }
        public void Select()
        {
            if (Selected != null)
                Selected.Invoke();
        }
        public delegate void OnSelectedHandler();
        public event OnSelectedHandler Selected;
    }
    public class SongSelectionSelection
    {
        public bool InSpecialButtons { get; set; }
        public bool InSongList { get; set; }
        public int SelectionIndex { get; set; }
        public SongSelectionSelection()
        {
            InSongList = true;
            SelectionIndex = 0;
        }
    }
    public class SongSelectionListItem
    {
        private string text;
        private object itemBehind;
        private int indentation;

        public string Text { get { return text; } }
        public object ItemBehind { get { return itemBehind; } }
        private bool highlightable, enabled;
        public bool Highlightable { get { return highlightable; } }
        public bool Enabled { get { return enabled; } }
        public int Indentation { get { return indentation; } }
        public SongSelectionListItem(object item, string text, bool highlightable, bool enabled, int indentation)
        {
            this.text = text;
            this.itemBehind = item;
            this.highlightable = highlightable;
            this.enabled = enabled;
            this.indentation = indentation;
        }
    }
}
