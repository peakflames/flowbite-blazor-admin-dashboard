/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "App.razor",
        "./wwwroot/**/*.{razor,html,cshtml,cs}",
        "./Layout/**/*.{razor,html,cshtml,cs}",
        "./Pages/**/*.{razor,html,cshtml,cs}",
        "./Components/**/*.{razor,html,cshtml,cs}"
    ],
    darkMode: 'class',
    safelist: [
        "md:bg-transparent",
        "md:block",
        "md:border-0",
        "md:dark:hover:bg-transparent",
        "md:dark:hover:text-white",
        "md:flex-row",
        "md:font-medium",
        "md:hidden",
        "md:hover:bg-transparent",
        "md:hover:text-primary-700",
        "md:mt-0",
        "md:p-0",
        "md:space-x-8",
        "md:text-primary-700",
        "md:text-sm",
        "md:w-auto",
        "h-56",
        "sm:h-64",
        "xl:h-80",
        "2xl:h-96",
        "z-10",
        "opacity-0",
        "opacity-100"
    ]
}