<a name="readme-top"></a>

# GitBook to PDF .NET Core
gitbook2pdf.net is a .NET Core console app that performs the complete convertion from GitBook URL to PDF. The URL provided must be the page containing the table of contents, in order to let the application to fetch all the chapters's URL and subsequently each HTML content.

## Build the project
Build using the .NET Core CLI, which is installed with [the .NET Core SDK](https://www.microsoft.com/net/download). Then run these commands from the CLI in the directory of the project:

```console
dotnet build
```

```console
dotnet run
```

These will install any needed dependencies, build the project, and run the project respectively.

## Roadmap
- [x] Add Changelog
- [x] Add License
- [x] Add new pdf component for HTML to PDF feature
- [ ] Fill assets/css/style.css to improve the look of the generated pdf
- [ ] Multi-language Support

See the [open issues](https://github.com/alciarro93/gitbook2pdf.net/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Contributing
Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some amazing feature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Changelog
See [`CHANGELOG.md`](https://github.com/alciarro93/gitbook2pdf.net/blob/main/CHANGELOG.md) for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## License
Distributed under the GNU General Public License version 3. See [`LICENSE.txt`](https://github.com/alciarro93/gitbook2pdf.net/blob/main/LICENSE.txt) for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Acknowledgments
This project works thanks to the following free and open source packages:
* [HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack/)
* [SimpleHtmlToPdf](https://www.nuget.org/packages/SimpleHtmlToPdf)

<p align="right">(<a href="#readme-top">back to top</a>)</p>
