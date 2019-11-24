![Banner](docs/banner.png)

An Elementary OS command line utility to set a random wallpaper from [wallhaven.cc](https://wallhaven.cc).

![Gif example](https://s5.gifyu.com/images/ezgif-1-a7d30ac4ef7d.gif)

## Options

### Sensitivity

| Option            | Description   |
| ----------------- |:-------------:|
| `-n` or `--nsfw`  | Picks a random sketchy wallpaper. |
| `-m` or `--mixed` | Picks a random sketchy or normal wallpaper. |

> Without any of the options, the default always picks a non-sketchy wallpaper

### Content

| Option            | Description   |
| ----------------- |:-------------:|
| `-a` or `--anime`  | Picks only anime wallpapers. |
| `-g` or `--general` | No anime, no people. |
| `-w` or `--wide` | Picks only 16x9 wallpapers. |
| `-search=[SEARCH TERM]` | Searches for a specified term. |

### Examples

```bash
theme-me-up --nsfw --wide -search=overwatch
```

> Sets a random sketchy overwatch wallpaper with the 16x9 aspect ratio

---

```bash
theme-me-up --general -search=id:338
```

> Sets a random non-sketchy wallpaper with the `night` tag see [wallhaven.cc/tags](https://wallhaven.cc/tags) for tag IDs.

### Why is the code so bad?

I just needed this utility and so I put it together in a couple of minutes.

I made this tool for my own use. But who knows, I might get bored and refactor this a bunch. Or you might.

Things to improve:

- Support all options wallhaven offers for filtering
- Maybe allow for caching of results
- Support other systems, currently made just for my Elementary OS (will probably run on Ubuntu).
